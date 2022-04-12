using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _sp;

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
}
