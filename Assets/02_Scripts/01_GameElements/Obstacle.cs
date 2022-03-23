using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactable
{
    public override bool Interact()
    {
        return false;
    }

    public override bool ExplosionInteract()
    {
        StartCoroutine(DelayedDestroy());

        return true;
    }

    IEnumerator DelayedDestroy()
    {
        yield return null;

        Destroy(gameObject);
    }
}
