using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float _time = 1.0f;

    private void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable;

        //Check if it's interactable
        if (!collision.TryGetComponent(out interactable))
        {
            return;
        }

        //Check if there's a wall in-between
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(collision.transform.position.x- transform.position.x, collision.transform.position.y -transform.position.y),
            Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
        if (raycastHit2D.transform != collision.transform)
            return;

        interactable.ExplosionInteract();
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
