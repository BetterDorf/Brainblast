using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] EventScriptableObject _playerMove;

    [Tooltip("Range in which the player is detected for the openning animation")]
    [SerializeField] float _detectionRange = 2.5f;

    [Header("Confetti")]
    [Tooltip("Object to create when the player wins")]
    [SerializeField] GameObject _confetti;
    [SerializeField] Vector3 _confettiOffset;

    [Header("Sounds")]
    [SerializeField] SoundRequests _soundReq;
    [SerializeField] AudioClip _clip;

    Animator _animator;
    bool _isOpen = false;

    private void Start()
    {
        _playerMove.OnEventTriggered += CheckIfPlayerIsClose;

        _animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        _playerMove.OnEventTriggered -= CheckIfPlayerIsClose;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;

        if (collision.TryGetComponent(out player))
        {
            //Make player win
            player.StartCoroutine(player.Win(transform.position));

            //Play particle effect
            Instantiate(_confetti, transform.position + _confettiOffset, Quaternion.identity);
        }
    }

    void CheckIfPlayerIsClose()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, Vector2.one * _detectionRange * 2, 0.0f, LayerMask.GetMask("Default"));

        //Check if player is within the found items
        if(Array.Exists(collider2Ds, (Collider2D col) => { return col.GetComponent<Player>() != null; }))
        {
            //Open the door
            OpenOrClose(true);
        }
        else if (_isOpen)
        {
            //Close the door if player isn't near and door was open
            OpenOrClose(false);
        }
    }
    
    /// <summary>
    /// Opens or closes the door (purely visual)
    /// </summary>
    /// <param name="open">Wether the door should be open or closed</param>
    void OpenOrClose(bool open)
    {
        if (_isOpen == open)
            return;

        //Play sound
        _soundReq.Request(_clip);

        //Switch the door's status
        _isOpen = open;

        //Update the animator
        _animator.SetBool("isOpen", _isOpen);
    }
}
