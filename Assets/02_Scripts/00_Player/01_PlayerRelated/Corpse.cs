using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] GameObject _explosionObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Create the explosions and destroy themselves
    /// </summary>
    public void Explode()
    {
        //Create first explosion
        Instantiate(_explosionObject, transform.position, Quaternion.identity);

        //Destroy ourselves
        Destroy(gameObject);
    }
}
