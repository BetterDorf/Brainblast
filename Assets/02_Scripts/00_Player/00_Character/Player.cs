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
    [Tooltip("How many times you should die solving the level")]
    [SerializeField] int _maxDeaths = 2;
    public int MaxDeaths { get { return _maxDeaths; } }
    //number of time the player has died playing the level
    int _deaths = 0;

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

    [Header("References")]
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

    [SerializeField] GameObject _smoke;

    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _collider2D = GetComponent<Collider2D>();

        //Create the life canvas
        _lifeCanvas = Instantiate(_lifeCanvasObject, Vector3.zero, Quaternion.identity);

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
        //_lives--;
        //UpdateLifeCounter();
        //if (_lives < 0)
        //{
        //    Lose();
        //    yield break;
        //}

        //Spawn the player
        if (_cloneVatReference)
            transform.position = _cloneVatReference.GameObject.transform.position;

        yield return new WaitForSeconds(_respawnTime / 2.0f);

        Instantiate(_smoke, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_respawnTime / 2.0f);

        //Change the state to allow the player to move again
         _state = PlayerState.Alive;

        //TODO update visuals
        GetComponentInChildren<SpriteRenderer>().enabled = true;
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
