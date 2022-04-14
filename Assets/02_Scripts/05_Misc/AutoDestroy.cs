using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Time the object will destroy itself after")]
    [SerializeField] float _destroyAfter = 1.0f;

    private void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_destroyAfter);

        Destroy(gameObject);
    }
}
