using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    PlayerInput _input;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
    }

    public void DieInput(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Die");
    }

    public void ExplodeCorpsesInput(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("ExplodeCorpses");
    }

    public void InteractInput(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Interact");
    }
}
