using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButton : Lever
{
    [SerializeField] EventScriptableObject _playerActionEvent;

    [Tooltip("Don't mess too much with this value, the goal is to be consistent across levels")]
    [SerializeField] int _stayPressedForTurns = 7;
    int _turnsToUnpress;

    int _itemsOnTheButton = 0;
    bool _isPressed = false;

    //UI
    [SerializeField] GameObject _countCanvas;

    private void Start()
    {
        _playerActionEvent.OnEventTriggered += OnPlayerAct;
        _revealAction.OnValueChanged += OnDrawChange;
    }

    private void OnDestroy()
    {
        _revealAction.OnValueChanged -= OnDrawChange;
        _playerActionEvent.OnEventTriggered -= OnPlayerAct;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Disable the UI if something is on the button
        _countCanvas.SetActive(false);

        //Add the item to the list of items on the button
        if (++_itemsOnTheButton > 0)
        {
            //Turn the button on
            if (!_isOn)
            {
                //Update the lines
                UpdateLines(_onColor);

                //Activate each element
                foreach (Activatable activatable in _linked)
                {
                    activatable.PowerOn();
                }

                //Update the visual
                GetComponent<SpriteRenderer>().sprite = _onSprite;

                //Play a sound
                GetComponent<AudioSource>()?.Play();
            }

            //set the button to pressed
            _isPressed = true;
            _isOn = true;

            //Reset the turns until the button gets unpressed
            _turnsToUnpress = _stayPressedForTurns;
            
            //Update ui value
            _countCanvas.GetComponentInChildren<Text>().text = _turnsToUnpress.ToString();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Depress the button if nothing is on it anymore
        if (--_itemsOnTheButton == 0)
        {
            //Set the ui
            _countCanvas.SetActive(true);

            //Change state
            _isPressed = false;
        }
    }

    public override bool Interact()
    {
        return false;
    }

    void OnPlayerAct()
    {
        //If the button isn't being pressed, decrease its count
        if (_isOn && !_isPressed)
        {
            _turnsToUnpress -= 1;

            //Update the UI
            _countCanvas.GetComponentInChildren<Text>().text = _turnsToUnpress.ToString();

            //Deactivate if count reaches 0
            if(_turnsToUnpress == 0)
            {
                _isOn = false;

                UpdateLines(_offColor);

                //Deactivate the ui
                _countCanvas.SetActive(false);

                //Deactivate each element
                foreach (Activatable activatable in _linked)
                {
                    activatable.PowerOff();
                }

                //Play a sound
                GetComponent<AudioSource>()?.Play();

                //Update the visual
                GetComponent<SpriteRenderer>().sprite = _offSprite;
            }
        }
    }
}
