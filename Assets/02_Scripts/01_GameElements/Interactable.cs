using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision?.GetComponent<Player>())
            Interact();
    }

    public abstract bool Interact();
    public virtual bool ExplosionInteract()
    {
        return Interact();
    }
}
