using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] SoundRequests _soundReq;
    [SerializeField] AudioClip _clip;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;

        if (collision.TryGetComponent(out player))
        {
            _soundReq.Request(_clip);
            player.ApplyAcid();
        }
    }
}
