using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _sp;

    [Header("Exit animation")]
    [Tooltip("Sprite used by the character when it exits")]
    [SerializeField] Sprite _exitSprite;
    [Tooltip("Distance to hover above exit before going through")]
    [SerializeField] Vector3 _exitOffset = Vector3.up;
    [Tooltip("Time that it takes to go to the exit")]
    [SerializeField] float _timeToExit = 0.1f;
    [Tooltip("Time to cross through the exit")]
    [SerializeField] float _timeToTraverseExit = 0.5f;

    [Tooltip("Used to hide part of the player")]
    [SerializeField] GameObject _mask;

    /// <summary>
    /// Make the character visible with his default color
    /// </summary>
    public void ResetVisuals()
    {
        _sp.enabled = true;
        _sp.color = Color.white;
    }

    /// <summary>
    /// Hide The character
    /// </summary>
    public void Hide()
    {
        _sp.enabled = false;
    }

    /// <summary>
    /// StartDisplaying the visuals for being affected with acid
    /// </summary>
    public void AcidEffect()
    {
        _sp.color = Color.green;
    }

    /// <summary>
    /// Begin a walk animation in a given direction
    /// </summary>
    /// <param name="direction">direction to walk in</param>
    public void StartWalkAnimation(Vector2 direction)
    {
        //Find out which direction we want to move then set the corresponding trigger on animator
        if (direction.x > 0)
        {
            _animator.SetTrigger("Right");
        }
        else if (direction.x < 0)
        {
            _animator.SetTrigger("Left");
        }
        else if (direction.y > 0)
        {
            _animator.SetTrigger("Up");
        }
        else if (direction.y < 0)
        {
            _animator.SetTrigger("Down");
        }
        else
        {
            //Give an error if the movement is (0/0)
            Debug.LogError("There is non movement to animate");
        }
    }

    public IEnumerator GoThroughExit(Vector3 exitPos)
    {
        //Change our sprite
        _animator.enabled = false;
        _sp.sprite = _exitSprite;

        float time = 0.0f;
        Vector3 firstPos = transform.position;

        //Go above the exit
        while(time < _timeToExit)
        {
            transform.position = Vector3.Lerp(firstPos, exitPos + _exitOffset, time / _timeToExit);

            yield return null;
            time += Time.deltaTime;
        }

        //Create mask
        var mask = Instantiate(_mask, exitPos, Quaternion.identity);

        //Go trough the exit
        time = 0.0f;
        firstPos = transform.position;
        while(time < _timeToTraverseExit)
        {
            transform.position = Vector3.Lerp(firstPos, exitPos - _exitOffset / 2.0f, time / _timeToTraverseExit);

            yield return null;
            time += Time.deltaTime;
        }
    }
}
