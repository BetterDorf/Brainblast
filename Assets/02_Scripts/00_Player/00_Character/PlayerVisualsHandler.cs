using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour
{
    [SerializeField] Animator _animator;

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
