using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Collider2D _collider2D;
    PlayerMovement _movement;

    [SerializeField] EventScriptableObject _playerActEvent;
    [SerializeField] PlayerVisualsHandler _visuals;

    [Header("Level-Specific Variables")]
    [Tooltip("How many times you should die solving the level")]
    [SerializeField] int _maxDeaths = 2;

    //number of time the player has died playing the level
    int _deaths = 0;
    //Number of turns the player took
    int _turns = 0;

    [Tooltip("Where the player first spawns")]
    [SerializeField] GameObject _firstCloneVat;

    //Player State
    PlayerState _state = PlayerState.Dead;
    public PlayerState State { get { return _state; } }
    public enum PlayerState
    {
        Alive,
        Melting,
        Dead
    }

    [Header("General Variables")]
    [SerializeField] int _acidTime = 7;
    [Tooltip("Time it takes for the player to respawn and be able to move again")]
    [SerializeField] float _respawnTime = 0.25f;
    [Tooltip("Time before we start respawning after dying")]
    [SerializeField] float _deathWaitTime = 0.25f;
    int _acidTurnsLeft;

    [Header("References")]
    [SerializeField] ObjectReferenceScriptableObject _cloneVatReference;

    [Header("UI")]
    [SerializeField] GameObject _loseCanvas;
    [SerializeField] GameObject _EndCanvas;
    [SerializeField] GameObject _lifeCanvasObject;
    GameObject _lifeCanvas;

    [SerializeField] GameObject _countCanvas;

    //Corpses
    [SerializeField] GameObject _corpse;
    List<GameObject> _corpses = new List<GameObject>();
    public List<GameObject> Corpses { get { return _corpses; } }
    public void ResetCorpses() { _corpses = new List<GameObject>(); }

    [SerializeField] GameObject _smoke;

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();

        //Create the life canvas
        _lifeCanvas = Instantiate(_lifeCanvasObject, Vector3.zero, Quaternion.identity);

        //Link the player actions with the acid countdown
        _playerActEvent.OnEventTriggered += OnPlayerAct;

        //Make sure the player has a start point
        if (!_firstCloneVat)
            Debug.LogError("There must be a starting vat assigned");

        //Select the clone vat
        _firstCloneVat.GetComponent<CloneVat>()?.Select();

        //Spawn for the first time
        StartCoroutine(Spawn(true));
    }

    /// <summary>
    /// Spawn the player
    /// </summary>
    /// <returns>true if player was spawned, false if she didn't have enough lives</returns>
    IEnumerator Spawn(bool firstSpawn = false)
    {
        //Update Visuals
        _visuals.Hide();

        //Spawn the player
        if (_cloneVatReference)
            transform.position = _cloneVatReference.GameObject.transform.position;

        //Wait to let the player 
        if (!firstSpawn)
            yield return new WaitForSeconds(_deathWaitTime);

        //Start the smoke effect
        Instantiate(_smoke, transform.position, Quaternion.identity);

        //Wait
        yield return new WaitForSeconds(_respawnTime);

        //Change the state to allow the player to move again
         _state = PlayerState.Alive;

        //update visuals
        _visuals.ResetVisuals();
    }

    /// <summary>
    /// Kill without leaving a corpse behind
    /// </summary>
    public void Melt()
    {
        if (_state == PlayerState.Dead)
        {
            return;
        }    

        Die();
    }
    
    /// <summary>
    /// Kill the player and leave a corpse
    /// </summary>
    public void Kill()
    {
        if (_state == PlayerState.Dead)
            return;

        _corpses.Add(Instantiate(_corpse, transform.position, Quaternion.identity));

        Die();
    }

    void Die()
    {
        //Make the player dead
        _state = PlayerState.Dead;
        _deaths++;

        //Update UI
        _lifeCanvas?.GetComponentInChildren<LifeUI>().UpdateLives(_deaths);

        //Make smoke appear
        Instantiate(_smoke, transform.position, Quaternion.identity);

        //Make the countdown ui disappear
        _countCanvas.SetActive(false);

        StartCoroutine(Spawn());
    }

    public void ApplyAcid()
    {
        //don't do anything if the player is already melting
        if (_state == PlayerState.Melting)
            return;

        //Make the player start melting
        _state = PlayerState.Melting;
        _acidTurnsLeft = _acidTime;

        //Update the player's visuals
        _visuals.AcidEffect();

        //Update the canvas's visuals
        _countCanvas.SetActive(true);
        _countCanvas.GetComponentInChildren<Text>().text = _acidTurnsLeft.ToString();
    }

    void OnPlayerAct()
    {
        CountDownAcid();
        _turns++;
    }

    void CountDownAcid()
    {
        //Don't do anything if the player isn't melting
        if (_state != PlayerState.Melting)
            return;

        //Decrease the turns left
        _acidTurnsLeft -= 1;

        //Update the canvas
        _countCanvas.GetComponentInChildren<Text>().text = _acidTurnsLeft.ToString();
        
        //die if we reach zero
        if (_acidTurnsLeft == 0)
            Kill();
    }

    /// <summary>
    /// Stop the play of the game and show the win canvas
    /// </summary>
    public void Win(Vector2 exitPosition)
    {
        //Disable the player
        _state = PlayerState.Dead;

        //Start animating the player to go through the door
        _visuals.GoThroughExit(exitPosition);

        //Hide now-useless UI
        _lifeCanvas.SetActive(false);
        transform.Find("UI").gameObject.SetActive(false);

        //Make the Win screen appear
        Instantiate(_EndCanvas).GetComponent<EndScreen>().ActivateEndScreen(_deaths, _maxDeaths, _turns);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Lose()
    {
        Instantiate(_loseCanvas);
    }

    private void OnDestroy()
    {
        _playerActEvent.OnEventTriggered -= OnPlayerAct;
    }
}
