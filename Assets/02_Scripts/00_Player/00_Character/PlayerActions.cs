using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] EventScriptableObject _playerActionEvent;
    [SerializeField] SharedIntScriptableObject _revealLinks;

    Player _player;
    PlayerMovement _movement;
    Collider2D _playerCollider;

    private void Awake()
    { 
        _player = GetComponent<Player>();
        _movement = GetComponent<PlayerMovement>();
        _playerCollider = GetComponent<Collider2D>();
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
        if (_player.Lives != 0)
            _player.Kill();
    }

    public void ExplodeInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead && _movement.StoppedMoving && !GameManager.PAUSED)
        {
            _playerActionEvent.TriggerEvent();
            StartCoroutine(ExplodeAction());
        }
    }

    IEnumerator ExplodeAction()
    {
        //grab corpses
        List<GameObject> corpses = _player.Corpses;

        //Remove null gameObjects
        corpses.RemoveAll(x => !x);

        ////Die before the explosion if we have no corpse
        //if (corpses.Count == 0)
        //{
        //    DieAction();
        //}

        //Explode the corpses
        foreach (GameObject corpse in corpses)
        {
            yield return new WaitForSeconds(0.05f);
            if(corpse != null)
                corpse.GetComponent<Corpse>()?.Explode();
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
