using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    //Door logic
    [Tooltip("Wether the door will be open at the start of the level. Makes the door close when activated")]
    [SerializeField] bool _startOpen = false;
    bool _isOpen = false;
    public bool IsOpen { get { return _isOpen; } }

    Collider2D _collider;

    //Visuals
    SpriteRenderer _sp;
    Sprite _closedSprite;
    [SerializeField] Sprite _openSprite;

    private void Start()
    {
        //Setup the visuals
        _sp = GetComponent<SpriteRenderer>();
        _closedSprite = _sp.sprite;

        //Grab the collider
        _collider = GetComponent<Collider2D>();

        //set the door to the correct start state
        _isOpen = _startOpen;
        UpdateDoor();
    }

    //Update the visual and the collider based on the bool _isOpen
    void UpdateDoor()
    {
        if (_isOpen)
        {
            //Change the visual
            _sp.sprite = _openSprite;

            //Change the physical properties
            _collider.enabled = false;
        }
        else
        {
            //Change the visual
            _sp.sprite = _closedSprite;

            //Change the physical properties
            _collider.enabled = true;
        }
    }

    protected override void Activate()
    {
        _isOpen = !_isOpen;
        UpdateDoor();
    }

    protected override void DeActivate()
    {
        _isOpen = !_isOpen;
        UpdateDoor();
    }
}
