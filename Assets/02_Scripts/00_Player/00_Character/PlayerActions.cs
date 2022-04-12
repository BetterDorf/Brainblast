using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [Header("Scriptable objects refs")]
    [SerializeField] EventScriptableObject _playerActionEvent;
    [SerializeField] SharedIntScriptableObject _revealLinks;

    Player _player;
    PlayerMovement _movement;
    Collider2D _playerCollider;

    [Header("Gameplay variables")]
    [Tooltip("Time to wait inbetween corpses explosion")]
    [SerializeField] float _timeBetweenExplosions = 0.0f;

    private void Awake()
    { 
        _player = GetComponent<Player>();
        _movement = GetComponent<PlayerMovement>();
        _playerCollider = GetComponent<Collider2D>();

        _revealLinks.Reset();
    }

    public void DieInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead && _movement.StoppedMoving && !GameManager.PAUSED)
        {
            _playerActionEvent.TriggerEvent();
            DieAction();
        }
    }

    void DieAction()
    {
        _player.Kill();
    }

    public void ExplodeInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead && _movement.StoppedMoving && !GameManager.PAUSED)
        {
            StartCoroutine(ExplodeAction());
        }
    }

    IEnumerator ExplodeAction()
    {
        //grab corpses
        List<GameObject> corpses = _player.Corpses;

        //Remove null gameObjects
        corpses.RemoveAll(x => !x);

        //Send ActionEvent only if there are corpses to explode
        if(corpses.Count != 0)
            _playerActionEvent.TriggerEvent();

        //Explode the corpses
        foreach (GameObject corpse in corpses)
        {
            //Explode the corpse if it isn't nulled
            if(corpse != null)
                corpse.GetComponent<Corpse>()?.Explode();

            //Wait a bit between explosions
            yield return new WaitForSeconds(_timeBetweenExplosions);
        }

        //Reset the corpses
        _player.ResetCorpses();
    }

    public void RevealInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead && _movement.StoppedMoving && !GameManager.PAUSED)
        {
            RevealAction();
        }
    }

    void RevealAction()
    {
        if (_revealLinks.Value == 0)
        {
            _revealLinks.ChangeValue(1);
        }
        else
        {
            _revealLinks.ChangeValue(0);
        }
    }

    public void ResetInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _movement.StoppedMoving && !GameManager.PAUSED)
            ResetAction();
    }

    void ResetAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
