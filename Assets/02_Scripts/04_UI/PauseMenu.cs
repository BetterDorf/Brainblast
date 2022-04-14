using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    bool _isOn = false;

    public void OnPause(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            PauseUnpause();
        }
    }

    void PauseUnpause()
    {
        if (GameManager.PAUSED)
        {
            if (_isOn)
            {
                //Unpause the game
                GameManager.UNPAUSE();

                //Hide the pause menu
                SetActiveChildren(false);

                _isOn = false;

                //Lock the cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            //Pause the game
            GameManager.PAUSE();

            //Show the pause menu
            SetActiveChildren(true);

            _isOn = true;

            //unlock the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// Set each child of this object to a active or unactive
    /// </summary>
    /// <param name="active">The state to set the children to</param>
    void SetActiveChildren(bool active)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        PauseUnpause();
    }
}
