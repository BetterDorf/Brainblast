using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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

    [SerializeField] ObjectReferenceScriptableObject _cloneVatReference;

    //Corpses
    [SerializeField] GameObject _corpse;
    List<GameObject> _corpses = new List<GameObject>();
    public List<GameObject> Corpses { get { return _corpses; } }
    public void ResetCorpses() { _corpses = new List<GameObject>(); }

    // Start is called before the first frame update
    void Start()
    {
        _firstCloneVat.GetComponent<CloneVat>()?.Select();
        Spawn();
    }

    /// <summary>
    /// Spawn the player
    /// </summary>
    /// <returns>true if player was spawned, false if she didn't have enough lives</returns>
    bool Spawn()
    {
        if (--_lives < 0)
            return false;

        //Change the state
        _state = PlayerState.Alive;

        //Spawn the player
        transform.position = _cloneVatReference.GameObject.transform.position;

        return true;
    }

    /// <summary>
    /// Kill without leaving a corpse behind
    /// </summary>
    public void Melt()
    {
        Die();
    }
    
    /// <summary>
    /// Kill the player and leave a corpse
    /// </summary>
    public void Kill()
    {
        _corpses.Add(Instantiate(_corpse, transform.position, Quaternion.identity));

        Die();
    }

    void Die()
    {
        StopAllCoroutines();

        _state = PlayerState.Dead;

        if (!Spawn())
            Lose();
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
