using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PromptsUpdater : MonoBehaviour
{
    public enum Device
    {
        Keyboard,
        Xbox,
        Ps
    }

    public void LinkInputSystem(ref PlayerInput input)
    {
        input.onControlsChanged += UpdatePrompts;
    }

    public void UpdatePrompts(PlayerInput input)
    {
        //find the device
        Device device;
        if (input.currentControlScheme == "Keyboard")
            device = Device.Keyboard;
        else if (input.devices[0].description.manufacturer == "Sony Interactive Entertainment" || input.devices[0].description.manufacturer == "Sony Computer Entertainment")
        {
            device = Device.Ps;
        }
        else
        {
            device = Device.Xbox;
        }

        //Update the prompts
        foreach (Transform child in transform)
        {
            child.GetComponent<Prompt>()?.UpdatePrompt(device);
        }
    }
}
