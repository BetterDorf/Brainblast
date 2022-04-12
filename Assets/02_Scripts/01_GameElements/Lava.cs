using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] SoundRequests _soundReq;
    [SerializeField] AudioClip _clip;

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        Player player;

        if (collision.TryGetComponent(out player))
        {
            _soundReq.Request(_clip);
            player.Melt();
        }
    }
}
