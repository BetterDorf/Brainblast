using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] EventScriptableObject _playerActionEvent;
    
    [Header("Sounds")]
    [SerializeField] SoundRequests _soundReq;
    [SerializeField] List<AudioClip> _stepsClips;
    [SerializeField] AudioClip _acidStepClip;

    PlayerInput _input;
    Player _player;
    PlayerVisualsHandler _visuals;

    [Header("Movement Settings")]
    [Tooltip("How quickly the character goes from tile to tile")]
    [SerializeField] float _speed;
    [Tooltip("The time the character waits when the player goes in the same direction")]
    [SerializeField] float _timeBetweenMoves;
    [Tooltip("How quickly we start buffering the input of the player after we started moving")]
    [SerializeField] float _bufferInputTreshold;

    //Dictate wether the player can use its normal movement
    bool _canMove = true;
    //Wether the player can move by interrupting its wait time
    bool _canInterrupt = true;
    //Wether the player should be reading inputs
    bool _canRegisterInput = true;

    bool _stoppedMoving = true;
    public bool StoppedMoving { get { return _stoppedMoving; } }

    //The input the player wants to do
    Vector2 _registeredInput = Vector2.zero;
    //The input the character is currently performing
    Vector2 _currentInput = Vector2.zero;


    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
        _visuals = GetComponentInChildren<PlayerVisualsHandler>();
    }

    private void FixedUpdate()
    {
        //Check movement only when we can move
        if (!_canRegisterInput || _player.State == Player.PlayerState.Dead)
            return;

        //Interrupt the wait time if we decide to go in another direction
        if (_canInterrupt && _registeredInput != Vector2.zero)
        {
            Move(_registeredInput);
            return;
        }

        //Register the user's input if we can't move
        Vector2 input = _input.actions["Movement"].ReadValue<Vector2>();

        //Move in only a single axis
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            input.y = 0;
        }
        else
        {
            input.x = 0;
        }

        //We only want to have a 1 in a direction
        input.Normalize();

        if (!_canMove)
        {
            //Only buffer input if another key was pressed
            if (input != _currentInput)
                _registeredInput = input;

            return;
        }

        //read the value from the playerinput
        Vector2 movement;
        if (_registeredInput != Vector2.zero)
        {
            movement = _registeredInput;
        }
        else
            movement = input;

        //Start moving if there's an input
        if (movement != Vector2.zero)
        {
            Move(movement);
        }
    }

    void Move(Vector2 movement)
    {
        //Flush out the buffered input
        _registeredInput = Vector2.zero;

        //Check if we can move in this direction
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, movement, 1.0f, LayerMask.GetMask("Walls", "Obstacle"));
        if (hit2D)
        {
            return;
        }

        //Security to make sure we aren't moving anymore
        StopAllCoroutines();

        //Register which input we are now using
        _currentInput = movement;

        //Start the movement
        StartCoroutine(GoTo(transform.position + (Vector3) movement));

        //Update the visual
        _visuals.StartWalkAnimation(movement);

        //Play the sound
        if (_player.State == Player.PlayerState.Melting)
            _soundReq.Request(_acidStepClip);
        else
            _soundReq.Request(_stepsClips[Random.Range(0, _stepsClips.Count)]);
    }

    IEnumerator GoTo(Vector3 goal)
    {
        //Block new movements while we are performing this one
        _canMove = false;
        _canRegisterInput = false;
        _canInterrupt = false;
        _stoppedMoving = false;

        Vector3 direction = goal - transform.position;

        float previousDistance, firstDistance, curDistance;
        firstDistance = curDistance = Vector3.Distance(transform.position, goal);

        //Move to the goal
        do
        {
            //Make sure we are alive
            if(_player.State == Player.PlayerState.Dead)
            {
                _stoppedMoving = true;
                _canInterrupt = true;
                _canMove = true;
                _canRegisterInput = true;
                yield break;
            }

            //Displace the character
            transform.position += direction * Mathf.Min(Time.deltaTime, 0.03f) * _speed;

            //update our progress
            previousDistance = curDistance;
            curDistance = Vector3.Distance(transform.position, goal);

            //Start registering player input at half the distance
            if (firstDistance / _bufferInputTreshold > curDistance)
            {
                _canRegisterInput = true;
            }

            //Pause for a frame
            yield return null;
        } while (curDistance < previousDistance); //as long as the distance to it keeps decreasing

        //Snap to our goal
        transform.position = goal;

        _stoppedMoving = true;

        //Inform the world that we took an action
        _playerActionEvent.TriggerEvent();

        yield return null;

        //mark a pause that can be interrupted
        _canInterrupt = true;
        yield return new WaitForSeconds(_timeBetweenMoves);

        //Reset the current performed input
        _currentInput = Vector2.zero;
        _canMove = true;
    }
}
