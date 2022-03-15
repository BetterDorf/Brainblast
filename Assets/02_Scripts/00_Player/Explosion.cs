using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float _time = 1.0f;

    private void Start()
    {
        StartCoroutine(DelayedDestroy());

        Collider2D[] collisions = { };
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), collisions);

        foreach (Collider2D col in collisions)
        {
            OnTriggerEnter2D(col);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable;

        if (!collision.TryGetComponent(out interactable))
        {
            return;
        }

        interactable.ExplosionInteract();
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
