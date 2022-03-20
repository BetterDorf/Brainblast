using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Collider2D _collider2D;

    [Header("Level-Specific Variables")]
    [Tooltip("How many lives you get to do the puzzles, including the first one")]
    [SerializeField] int _lives = 2;
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
    [SerializeField] float _acidTime = 1.0f;
    [SerializeField] float _respawnTime = 0.25f;

    [SerializeField] ObjectReferenceScriptableObject _cloneVatReference;
    [SerializeField] GameObject _lifeCanvasObject;
    GameObject _lifeCanvas;

    //Corpses
    [SerializeField] GameObject _corpse;
    List<GameObject> _corpses = new List<GameObject>();
    public List<GameObject> Corpses { get { return _corpses; } }
    public void ResetCorpses() { _corpses = new List<GameObject>(); }

    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();

        //TODO do this another way
        _lifeCanvas = Instantiate(_lifeCanvasObject, Vector3.zero, Quaternion.identity);

        //TODO Move this somewhere else
        Cursor.visible = false;

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
            Lose();

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
        if (_state == PlayerState.Dead)
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
        StopAllCoroutines();

        _state = PlayerState.Dead;

        StartCoroutine(Spawn());
    }

    public void ApplyAcid()
    {
        if (_state == PlayerState.Melting)
            return;

        StartCoroutine(DelayedDie(_state = PlayerState.Melting, _acidTime));
    }

    /// <summary>
    /// Kill the player after a set time
    /// </summary>
    /// <param name="affliction">The state that the player must match to die</param>
    IEnumerator DelayedDie(PlayerState affliction, float time)
    {
        yield return new WaitForSeconds(time);

        if (_state == affliction)
        {
            Kill();
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
