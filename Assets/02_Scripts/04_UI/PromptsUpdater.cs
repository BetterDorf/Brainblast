using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptsUpdater : MonoBehaviour
{
    public void LinkInputSystem(ref PlayerInput input)
    {
        input.onControlsChanged += UpdatePrompts;
    }

    public void UpdatePrompts(PlayerInput input)
    {
        bool keyboard = true;
        if (input.currentControlScheme != "Keyboard")
            keyboard = false;

        foreach (Transform child in transform)
        {
            child.GetComponent<Prompt>()?.UpdatePrompt(keyboard);
        }
    }
}
