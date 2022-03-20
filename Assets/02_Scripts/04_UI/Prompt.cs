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
    [SerializeField] Sprite _psImage;

    //The image to change
    Image _image;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
        UpdatePrompt();
    }

    public void UpdatePrompt(PromptsUpdater.Device device = PromptsUpdater.Device.Keyboard)
    {
        if (_image == null)
            return;

        switch(device)
        {
            case PromptsUpdater.Device.Keyboard:
                _image.sprite = _keyboardImage;
                break;
            case PromptsUpdater.Device.Xbox:
                _image.sprite = _gamepadImage;
                break;
            case PromptsUpdater.Device.Ps:
                _image.sprite = _psImage;
                break;
            default:
                _image.sprite = _keyboardImage;
                break;
        }
    }
}
