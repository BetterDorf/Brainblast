using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public void OnPause(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            if (GameManager.PAUSED)
            {
                //Unpause the game
                GameManager.UNPAUSE();

                //Hide the pause menu
                SetActiveChildren(false);
            }
            else
            {
                //Pause the game
                GameManager.PAUSE();

                //Show the pause menu
                SetActiveChildren(true);
            }
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
}
