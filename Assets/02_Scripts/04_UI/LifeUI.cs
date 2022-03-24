using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [Header("Animations Values")]
    [Tooltip("How many times bigger the ui gets when animating")]
    [SerializeField] float _bigSizeMult;
    [Tooltip("from 0 to 1 how opaque the ui is when big")]
    [SerializeField] float _bigTransparency;
    [Tooltip("How fast the transition plays")]
    [SerializeField] float _transitionSpeed;
    [Tooltip("The margin at which we stop lerping")]
    [SerializeField] float _arrivalMargin;
    [Tooltip("How long to wait after we biggified the ui to animate the change")]
    [SerializeField] float _waitFor;
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
        //Make sure we aren't already animating our ui
        if (!_updatingUI)
            StartCoroutine(ChangeUI(newLives));
        else
        {
            //Remember the value we wanted to put
            _bufferedValues.Add(newLives);

            //If it's the only value we have buffered, we start to try to execute them quickly
            if (_bufferedValues.Count == 1)
            {
                StartCoroutine(UpdateUIWhenPossible());
            }
        }
    }

    /// <summary>
    /// Updates Ui as soon as possible (ie when _updatingUI is false)
    /// </summary>
    IEnumerator UpdateUIWhenPossible()
    {
        while(_bufferedValues.Count != 0)
        {
            //Wait until we aren't updating the ui anymore
            yield return new WaitUntil(() => _updatingUI == false);

            //Perform the animation/update
            yield return StartCoroutine(ChangeUI(_bufferedValues[0]));
            //remove the used value
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
        //Block other changes while we are animating this change
        _updatingUI = true;

        //Convert int to string
        string newValue = newLives.ToString();

        //Make a copy so we don't see the original disappearing
        GameObject tempUI = Instantiate(_lifeTransform.gameObject, transform);

        //Bigify the ui
        _lifeTransform.anchoredPosition = Vector2.zero;
        _lifeTransform.localScale *= _bigSizeMult;

        //Make the bigUI slightly transparent
        GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, _bigTransparency);
        foreach(Text text in GetComponentsInChildren<Text>())
        {
            text.color = new Color(0f, 0f, 0f, _bigTransparency);
        }

        //hold the ui for x seconds before animatingv
        yield return new WaitForSeconds(_waitFor);

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

        //Reset the colors
        GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
        foreach (Text text in GetComponentsInChildren<Text>())
        {
            text.color = new Color(0f, 0f, 0f, 1f);
        }

        //Destroy the temporary ui created
        Destroy(tempUI);

        //Stop animating
        _updatingUI = false;
    }

    IEnumerator TransitionUI()
    {
        //Set the goal position for the next value text to be the current value position
         Vector3 goalPosition = _curLife.position;
        //remember the offset between them
        Vector3 offset = _curLife.position - _nextLife.position;

        //Shift the nextValue text towards its goal until it's close enough
        while(Vector3.Distance(goalPosition, _nextLife.position) > _arrivalMargin)
        {
            //Wait a frame
            yield return null;

            //Lerp nextLife towards goal
            _nextLife.position = Vector3.Lerp(_nextLife.position, goalPosition, Mathf.Min(Time.deltaTime, 0.05f) * _transitionSpeed);
            //Shift curLife by same amount
            _curLife.position = _nextLife.position + offset;
        }

        //Snap the position
        _nextLife.position = goalPosition;
    }
}
