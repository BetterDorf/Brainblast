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
        Destroy(gameObject);

        return true;
    }
}
