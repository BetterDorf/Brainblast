using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] float _interactRadius = 1.0f;

    PlayerInput _input;
    Player _player;
    Collider2D _playerCollider;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
        _playerCollider = GetComponent<Collider2D>();
    }

    public void DieInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            _player.Kill();
    }

    public void ExplodeCorpsesInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            ExplodeCorpses();
        }
    }

    void ExplodeCorpses()
    {
        //grab corpses
        List<GameObject> corpses = _player.Corpses;
        corpses.RemoveAll(x => !x);

        //Explode the corpses
        foreach (GameObject corpse in corpses)
        {
            corpse.GetComponent<Corpse>().Explode();
        }

        //Reset the corpses
        _player.ResetCorpses();
    }

    public void InteractInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            Interact();
    }

    void Interact()
    {
        Collider2D[] colliders =
        Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), _interactRadius);

        Interactable closestInteractable = null;
        float distance = _interactRadius + 1.0f;

        //Figure out the closest interactable in range
        foreach (Collider2D collider in colliders)
        {
            if (collider != _playerCollider && collider.Distance(_playerCollider).distance < distance)
            {
                Interactable interactable;
                if (collider.TryGetComponent(out interactable))
                {
                    closestInteractable = interactable;
                }
            }
        }

        //Interact with the closest interactable
        if  (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }

    public void ResetInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
            ResetLevel();
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
