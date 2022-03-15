using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : Lava
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;

        if (collision.TryGetComponent(out player))
        {
            player.ApplyAcid();
        }
    }
}
