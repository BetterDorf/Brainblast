using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : Lever
{
    [SerializeField] EventScriptableObject _playerActionEvent;

    [Tooltip("Don't mess too much with this value, the goal is to be consistent across levels")]
    [SerializeField] int _stayPressedForTurns = 7;
    int _turnsToUnpress;

    int _itemsOnTheButton = 0;
    bool _isPressed = false;

    private void Start()
    {
        _playerActionEvent.OnEventTriggered += OnPlayerAct;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (++_itemsOnTheButton > 0)
        {
            if (!_isOn)
            {
                DrawLines(Color.green);

                foreach (Activatable activatable in _linked)
                {
                    activatable.PowerOn();
                }
            }

            //set the button to pressed
            _isPressed = true;
            _isOn = true;

            //Reset the turns until the button gets unpressed
            _turnsToUnpress = _stayPressedForTurns;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (--_itemsOnTheButton == 0)
            _isPressed = false;
    }

    public override bool Interact()
    {
        return false;
    }

    void OnPlayerAct()
    {
        if (_isOn && !_isPressed)
            if(--_turnsToUnpress == 0)
            {
                _isOn = false;

                DrawLines(Color.red);
                foreach (Activatable activatable in _linked)
                {
                    activatable.PowerOff();
                }
            }
    }
}
