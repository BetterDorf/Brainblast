using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody2D _rb;
    Player _player;

    [Header("Movement Settings")]
    [SerializeField] float _acceleration = 1.0f;
    [SerializeField] float _maxSpeed = 1.0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        Vector2 movement;
        if (_player.State == Player.PlayerState.Dead)
            movement = Vector2.zero;
        else
            movement = _input.actions["Movement"].ReadValue<Vector2>();

        _rb.velocity = Vector2.Lerp(_rb.velocity, movement * _maxSpeed, _acceleration * Time.fixedDeltaTime);
    }
}
