using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    //Wether this is activated or not
    [SerializeField] protected bool _isOn = false;

    [Tooltip("Elements that this will activate")]
    [SerializeField] protected List<Activatable> _linked = new List<Activatable>();

    [Tooltip("Line object used to draw lines to the linked objects")]
    [SerializeField] GameObject _line;
    List<GameObject> _lines = new List<GameObject>();

    private void Start()
    {
        DrawLines(Color.red);
    }

    //Draw lines to the linked objects
    protected void DrawLines(Color color)
    {
        //Reset the colors
        foreach (GameObject lineObject in _lines)
        {
            Destroy(lineObject);
        }
        _lines = new List<GameObject>();

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

    public override bool Interact()
    {
        _isOn = !_isOn;
        DrawLines(_isOn ? Color.green : Color.red);

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