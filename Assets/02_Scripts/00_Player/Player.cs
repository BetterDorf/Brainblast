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

    [Header("Level-Specific Variables")]
    [Tooltip("How many lives you get to do the puzzle, including the first life")]
    [SerializeField] int _lives = 2;
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
    [SerializeField] float _respawnTime = 0.25f;
    int _acidTurnsLeft;

    [SerializeField] ObjectReferenceScriptableObject _cloneVatReference;

    //UI
    [SerializeField] GameObject _loseCanvas;
    [SerializeField] GameObject _lifeCanvasObject;
    GameObject _lifeCanvas;

    [SerializeField] GameObject _countCanvas;


    //Corpses
    [SerializeField] GameObject _corpse;
    List<GameObject> _corpses = new List<GameObject>();
    public List<GameObject> Corpses { get { return _corpses; } }
    public void ResetCorpses() { _corpses = new List<GameObject>(); }

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();

        //TODO do this another way
        _lifeCanvas = Instantiate(_lifeCanvasObject, Vector3.zero, Quaternion.identity);

        //TODO Move this somewhere else
        Cursor.visible = false;

        //Link the player actions with the acid countdown
        _playerActEvent.OnEventTriggered += CountDownAcid;

        //Make sure the player has a start point
        if (!_firstCloneVat)
            Debug.LogError("There must be a starting vat assigned");

        _firstCloneVat.GetComponent<CloneVat>()?.Select();

        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Spawn the player
    /// </summary>
    /// <returns>true if player was spawned, false if she didn't have enough lives</returns>
    IEnumerator Spawn()
    {
        //TODO update visuals
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        //Lose a life
        _lives--;
        UpdateLifeCounter();
        if (_lives < 0)
        {
            Lose();
            yield break;
        }

        //Spawn the player
        if (_cloneVatReference)
            transform.position = _cloneVatReference.GameObject.transform.position;

        yield return new WaitForSeconds(_respawnTime);

        //Change the state to allow the player to move again
         _state = PlayerState.Alive;

        //TODO update visuals
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    void UpdateLifeCounter()
    {
        _lifeCanvas.GetComponentInChildren<Text>().text = "x" + _lives.ToString();
    }

    /// <summary>
    /// Kill without leaving a corpse behind
    /// </summary>
    public void Melt()
    {
        if (_state == PlayerState.Dead || !_movement.StoppedMoving)
            return;

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
        //StopAllCoroutines();

        //Make the player dead
        _state = PlayerState.Dead;

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

        //Update the canvas's visuals
        _countCanvas.SetActive(true);
        _countCanvas.GetComponentInChildren<Text>().text = _acidTurnsLeft.ToString();
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
    /// Exit the level
    /// </summary>
    public void ExitLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Lose()
    {
        Instantiate(_loseCanvas);
    }

    private void OnDestroy()
    {
        _playerActEvent.OnEventTriggered -= CountDownAcid;
    }
}
