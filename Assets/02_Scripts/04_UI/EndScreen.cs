using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [Header("Feedbacks")]
    [Tooltip("Lists of possible feedbacks from best to worst. First is impossible to get")]
    [SerializeField] List<string> _feedbacks;
    [Tooltip(("How many clones off you need to be to reach the next feedback"))]
    [SerializeField] int _stepSize = 1;

    [Tooltip("Object holding what we display")]
    [SerializeField] GameObject _notepad;

    [Header("Texts on the notepad")]
    [Tooltip("Text for the number of clones used")]
    [SerializeField] TMP_Text _clonesUsedText;
    [Tooltip("Text for an appreciation in english to the player")]
    [SerializeField] TMP_Text _feedbackText;
    [Tooltip("Text to display the 'time' (number of turns)")]
    [SerializeField] TMP_Text _timeText;


    [Header("Testing")]
    [SerializeField] bool _test;
    [SerializeField] int _testClones;
    [SerializeField] int _testidealClones;
    [SerializeField] int _testTime;

    private void Start()
    {
        if (_test)
        {
            ActivateEndScreen(_testClones, _testidealClones, _testTime);
        }
    }

    public void ActivateEndScreen(int clonesUsed, int idealClones, int turnsTaken)
    {
        //Set the numerical values
        _clonesUsedText.text = clonesUsed.ToString();
        _timeText.text = turnsTaken.ToString();

        //Set the feedback
        int feedbackNum;

        //Bypass index calculation if we did better than ideal
        if (clonesUsed < idealClones)
            feedbackNum = 0;
        else
        {
            //index is the number of excendentary clones we used divided by how many are needed to go to the next feedback.
            //We add 1 to avoid the "impossible" feedback
            feedbackNum = Mathf.CeilToInt((clonesUsed - idealClones) / (float) _stepSize) + 1;
        }

        //If there is a corresponding feedback, use it
        if (feedbackNum >= _feedbacks.Count)
            _feedbackText.text = _feedbacks[_feedbacks.Count - 1];
        else
            //Fall back to the last feedback
            _feedbackText.text = _feedbacks[feedbackNum];

        _notepad.SetActive(true);
    }
}