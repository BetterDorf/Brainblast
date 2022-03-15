using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    [SerializeField] bool _startOpen = false;
    bool _isOpen = false;
    public bool IsOpen { get { return _isOpen; } }

    Collider2D _collider;

    //For color change
    Color _baseColor;
    Color _openColor;

    private void Start()
    {
        _baseColor = GetComponent<SpriteRenderer>().color;
        _openColor = new Color(_baseColor.r, _baseColor.g, _baseColor.b, _baseColor.a / 2.5f);

        _collider = GetComponent<Collider2D>();

        _isOpen = _startOpen;
        UpdateDoor();
    }

    //Update the visual and the collider based on the bool _isOpen
    void UpdateDoor()
    {
        if (_isOpen)
        {
            GetComponent<SpriteRenderer>().color = _openColor;
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
            GetComponent<SpriteRenderer>().color = _baseColor;
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
