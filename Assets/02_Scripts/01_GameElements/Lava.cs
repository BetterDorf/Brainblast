using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        Player player;

        if (collision.TryGetComponent(out player))
        {
            player.Melt();
        }
    }
}
