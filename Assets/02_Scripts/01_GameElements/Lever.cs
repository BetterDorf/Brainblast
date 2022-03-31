using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    //Wether this is activated or not
    protected bool _isOn = false;

    [Tooltip("Elements that this will activate")]
    [SerializeField] protected List<Activatable> _linked = new List<Activatable>();

    [Header("Visuals")]
    [SerializeField] protected Sprite _onSprite;
    [SerializeField] protected Sprite _offSprite;

    [Tooltip("Line object used to draw lines to the linked objects")]
    [SerializeField] GameObject _line;
    List<GameObject> _lines = new List<GameObject>();
    [Tooltip("Color of the lines when this is turned on")]
    [SerializeField] protected Color _onColor;
    [Tooltip("Color of the lines when this is turned off")]
    [SerializeField] protected Color _offColor;
    [Tooltip("Scriptable object used to link to the player's actions")]
    [SerializeField] protected SharedIntScriptableObject _revealAction;

    private void Start()
    {
        _revealAction.OnValueChanged += OnDrawChange;
    }

    private void OnDestroy()
    {
        _revealAction.OnValueChanged -= OnDrawChange;
    }

    protected void OnDrawChange(int value)
    {
        UpdateLines(_isOn ? _onColor : _offColor);
    }

    //Draw lines to the linked objects
    protected void UpdateLines(Color color)
    {
        //Reset the colors
        EraseLines();

        //Draw lines only if reveal is on
        if (_revealAction.Value != 0)
        {
            //Draw a line per linked object
            foreach (Activatable activatable in _linked)
            {
                GameObject lineObject = Instantiate(_line, transform);
                lineObject.GetComponent<LineRenderer>().SetPositions(
                    new Vector3[] { transform.position, activatable.transform.position });

                //Change the color of the line
                lineObject.GetComponent<LineRenderer>().startColor = color;

                _lines.Add(lineObject);
            }
        }
    }

    protected void EraseLines()
    {
        foreach (GameObject lineObject in _lines)
        {
            Destroy(lineObject);
        }
        _lines = new List<GameObject>();
    }

    public override bool Interact()
    {
        //Switch the current state
        _isOn = !_isOn;

        //Update the line going to the linked elements
        UpdateLines(_isOn ? _onColor : _offColor);

        //Update the powered state of each linked element
        foreach (Activatable activatable in _linked)
        {
            if (_isOn)
                activatable.PowerOn();
            else
                activatable.PowerOff();
        }

        //Play a sound
        GetComponent<AudioSource>()?.Play();

        //Visual
        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().sprite = _isOn ? _onSprite : _offSprite;

        //Confirm that we were interacted with
        return true;
    }
}