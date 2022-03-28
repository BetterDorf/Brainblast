using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoSelect : MonoBehaviour
{
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Select();
    }

    private void OnEnable()
    {
        Select();
    }

    void Select()
    {
        _button.Select();
    }
}
