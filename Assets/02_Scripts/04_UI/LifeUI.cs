using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [Header("Animations Values")]
    [Tooltip("How many times bigger the ui gets when animating")]
    [SerializeField] float _bigSizeMult;
    [Tooltip("How fast the transition plays")]
    [SerializeField] float _transitionSpeed;
    [Tooltip("The margin at which we stop lerping")]
    [SerializeField] float _arrivalMargin;
    [Tooltip("How long we hold the ui in the center before returning to the normal positions")]
    [SerializeField] float _holdFor;

    [SerializeField] RectTransform _curLife;
    Vector3 _curLifeStartPosition;

    [SerializeField] RectTransform _nextLife;
    Vector3 _nextLifeStartPosition;

    //RectTransform of the parent object that holds the ui
    [SerializeField] RectTransform _lifeTransform;
    //Starting position
    Vector3 _lifeStartPosition;
    //Starting scale
    Vector3 _lifeStartScale;

    //wether or not we are currently animating a transition
    bool _updatingUI = false;
    //The list of values we have yet to animate
    List<int> _bufferedValues = new List<int>();

    private void Awake()
    {
        //Register the starting positions of our ui
        _lifeStartPosition = _lifeTransform.position;
        _lifeStartScale = _lifeTransform.localScale;

        _curLifeStartPosition = _curLife.position;
        _nextLifeStartPosition = _nextLife.position;
    }

    /// <summary>
    /// Update the ui with the new amount of lives
    /// </summary>
    /// <param name="newLives">Amount of lives we now have</param>
    public void UpdateLives(int newLives)
    {   
        if (!_updatingUI)
            StartCoroutine(ChangeUI(newLives));
        else
        {
            _bufferedValues.Add(newLives);

            if (_bufferedValues.Count == 1)
            {
                StartCoroutine(UpdateUIWhenPossible());
            }
        }
    }

    IEnumerator UpdateUIWhenPossible()
    {
        while(_bufferedValues.Count != 0)
        {
            //Wait until we aren't updating the ui anymore
            yield return new WaitUntil(() => _updatingUI == false);

            yield return StartCoroutine(ChangeUI(0));
            _bufferedValues.RemoveAt(0);
        }
    }

    public void UpdateLivesWithoutAnimating(int newLives)
    {
        //Convert int to string
        string newValue = newLives.ToString();

        //Change the value of the usual visible ui
        _curLife.GetComponent<Text>().text = newValue;
    }

    IEnumerator ChangeUI(int newLives)
    {
        _updatingUI = true;

        //Convert int to string
        string newValue = newLives.ToString();

        //Bigify the ui
        _lifeTransform.anchoredPosition = Vector2.zero;
        _lifeTransform.localScale *= _bigSizeMult;
        yield return new WaitForSeconds(_holdFor);


        //Change the visual value for the animation
        _nextLife.GetComponent<Text>().text = newValue;

        //Animate the change
        yield return StartCoroutine(TransitionUI());

        //Change the value of the usual visible ui
        _curLife.GetComponent<Text>().text = newValue;

        //Hold the ui a bit
        yield return new WaitForSeconds(_holdFor);

        //Reset the position
        _lifeTransform.position = _lifeStartPosition;
        _lifeTransform.localScale = _lifeStartScale;

        _curLife.position = _curLifeStartPosition;
        _nextLife.position = _nextLifeStartPosition;


        _updatingUI = false;
    }

    IEnumerator TransitionUI()
    {
        Vector3 goalPosition = _curLife.position;
        Vector3 offset = _curLife.position - _nextLife.position;

        while(Vector3.Distance(goalPosition, _nextLife.position) > _arrivalMargin)
        {
            _nextLife.position = Vector3.Lerp(_nextLife.position, goalPosition, Time.deltaTime * _transitionSpeed);
            _curLife.position = _nextLife.position + offset;

            yield return null;
        }
    }
}
