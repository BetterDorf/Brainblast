using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : Lever
{
    [Tooltip("Don't mess too much with this value, the goal is to be consistent across levels")]
    [SerializeField] float _stayPressedForSeconds = 3.5f;

    public override bool Interact()
    {
        if (_isOn)
            return false;

        _isOn = !_isOn;
        DrawLines(Color.green);

        foreach (Activatable activatable in _linked)
        {
            activatable.PowerOn();
        }

        StartCoroutine(DelayedDeactivation());

        return true;
    }

    IEnumerator DelayedDeactivation()
    {
        yield return new WaitForSeconds(_stayPressedForSeconds);

        _isOn = false;
        DrawLines(Color.red);
        foreach (Activatable activatable in _linked)
        {
            activatable.PowerOff();
        }
    }
}
