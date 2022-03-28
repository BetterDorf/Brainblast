using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Tooltip("The amount of tiles the explosion will expand to")]
    [SerializeField] int _power = 2;
    public int Power { get => _power; set => _power = value; }

    [Tooltip("Time the explosion will last")]
    [SerializeField] float _time = 1.0f;
    [Tooltip("Time to wait before this explosion propagates")]
    [SerializeField] float _timeBeforeSpawn = 0.2f;

    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _explosionObject;

    private void Start()
    {
        StartCoroutine(DelayedDestroy());

        StartCoroutine(SpawnExplosions());

        //Todo
        //Check for interactable and player to interact with
        RaycastHit2D[] hit2Ds = new RaycastHit2D[2];
        _collider.Raycast(Vector2.up, hit2Ds, 0.1f);

        //interact with everything that we overlapped
        foreach(RaycastHit2D hit2D in hit2Ds)
        {
            if (hit2D)
            {
                hit2D.collider.GetComponent<Player>()?.Melt();
                hit2D.collider.GetComponent<Interactable>()?.ExplosionInteract();
            }
        }
    }

    IEnumerator SpawnExplosions()
    {
        yield return new WaitForSeconds(_timeBeforeSpawn);

        //Check if we can propagate
        if (_power == 0)
        {
            yield break;
        }

        //Create direction vector
        Vector2 direction = Vector2.up;

        do
        {
            //Check if tile pointed is empty
            RaycastHit2D[] hit2Ds = new RaycastHit2D[3];
            int hits = _collider.Raycast(direction, hit2Ds, 1.0f, ~LayerMask.GetMask("Water", "Ignore Raycast"));

            //Create an explosion if it is empty
            if (hits == 0)
            {
                GameObject explo = Instantiate(_explosionObject, transform.position + (Vector3)direction, Quaternion.identity);

                //Make the power of that explosion to be 1 less
                explo.GetComponent<Explosion>().Power = _power - 1;
            }
            else
            {
                //Create an explosion anyway if what we hit isn't a wall
                bool canExplode = true;
                foreach (RaycastHit2D hit2D in hit2Ds)
                {
                    if (hit2D)
                    {
                        //This is super ugly and i don't understand bitwise magic but it checks if our object's layer is
                        //in the layermask
                        int layerMask = LayerMask.GetMask("Walls", "Explosion");
                        int layer = hit2D.collider.gameObject.layer;
                        if (layerMask == (layerMask | (1 << layer)))
                        {
                            canExplode = false;
                        }
                    }
                }

                if (canExplode)
                {
                    GameObject explo = Instantiate(_explosionObject, transform.position + (Vector3)direction, Quaternion.identity);

                    //Make the power of that explosion to be 0
                    explo.GetComponent<Explosion>()._power = 0;
                }
            }

            //Rotate direction vector if we aren't done
            direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90) * direction.x - Mathf.Sin(Mathf.Deg2Rad * 90) * direction.y,
                Mathf.Sin(Mathf.Deg2Rad * 90) * direction.x + Mathf.Cos(Mathf.Deg2Rad * 90) * direction.y);

        } while (direction != Vector2.up);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Interactable interactable = null;
    //    Player player = null;

    //    //Check if it's interactable
    //    if (!collision.TryGetComponent(out interactable))
    //    {
    //        //Check it it's the player
    //        if (!collision.TryGetComponent(out player))
    //            return;
    //    }

    //    //Check if there's a wall in-between or another object that would block us
    //    RaycastHit2D raycastHit2D = Physics2D.Raycast(
    //        new Vector2(transform.position.x, transform.position.y),
    //        new Vector2(collision.transform.position.x- transform.position.x, collision.transform.position.y -transform.position.y),
    //        Mathf.Infinity, ~LayerMask.GetMask("Water", "Ignore Raycast"));
    //    if (raycastHit2D.transform != collision.transform)
    //        return;

    //    //Interact with interactable
    //    interactable?.ExplosionInteract();

    //    //Kill the player
    //    player?.Melt();
    //}

    /// <summary>
    /// Destroy the explosion object after a time
    /// </summary>
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
