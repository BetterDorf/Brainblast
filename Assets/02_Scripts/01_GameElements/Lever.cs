using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] protected bool _isOn = false;

    [SerializeField] protected List<Activatable> _linked = new List<Activatable>();

    public override bool Interact()
    {
        _isOn = !_isOn;

        foreach (Activatable activatable in _linked)
        {
            if (_isOn)
                activatable.PowerOn();
            else
                activatable.PowerOff();
        }

        return true;
    }
}