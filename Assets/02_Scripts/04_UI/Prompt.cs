using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Prompt : MonoBehaviour
{
    [Header("Imgs for different inputs")]
    [SerializeField] Sprite _keyboardImage;
    [SerializeField] Sprite _gamepadImage;

    //The image to change
    Image _image;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        UpdatePrompt(true);
    }

    public void UpdatePrompt(bool defaultImg)
    {
        if (_image == null)
            return;

        if (defaultImg)
        {
            _image.sprite = _keyboardImage;
        }
        else
        {
            _image.sprite = _gamepadImage;
        }
    }
}
