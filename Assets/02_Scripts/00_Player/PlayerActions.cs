using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    EventScriptableObject _playerActionEvent;

    Player _player;
    Collider2D _playerCollider;

    private void Awake()
    { 
        _player = GetComponent<Player>();
        _playerCollider = GetComponent<Collider2D>();
    }

    public void DieInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead)
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
        if (callbackContext.performed && _player.State != Player.PlayerState.Dead)
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

        //Die before the explosion if we have no corpse
        if (corpses.Count == 0)
        {
            DieAction();
        }

        //Explode the corpses
        foreach (GameObject corpse in corpses)
        {
            yield return new WaitForSeconds(0.05f);
            corpse.GetComponent<Corpse>().Explode();
        }

        //Reset the corpses
        _player.ResetCorpses();
    }

    public void ResetInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            ResetAction();
    }

    void ResetAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
