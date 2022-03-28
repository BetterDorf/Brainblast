using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TitleCard : MonoBehaviour
{
    [Header("Variable text")]
    [Tooltip("Number of the level")]
    [SerializeField] int _levelNumber;
    [Tooltip("Name of the level")]
    [SerializeField] string _levelName;
    [Tooltip("Narration to accompagny")]
    [SerializeField] string _narration;

    [Header("Fixed text")]
    [Tooltip("The string part before the level number")]
    [SerializeField] string _levelIndicator = "Level - ";
    [Tooltip("Seperation between level number and title")]
    [SerializeField] string _titleSeperator = "\n --- \n";

    [Header("References")]
    [Tooltip("Text to put the title in")]
    [SerializeField] TextMeshProUGUI _titleText;
    [Tooltip("Text to put the narration in")]
    [SerializeField] TextMeshProUGUI _narrationText;

    private void Start()
    {
        _titleText.text = _levelIndicator + _levelNumber.ToString() + _titleSeperator + _levelName;

        _narrationText.text = _narration;
    }

    private void Update()
    {
    }
}
