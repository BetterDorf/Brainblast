using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("The amount of tiles the explosion will expand to")]
    [SerializeField] int _power = 2;
    public int Power { get => _power; set => _power = value; }

    [Tooltip("Time the explosion will last")]
    [SerializeField] float _time = 1.0f;
    [Tooltip("Time to wait before this explosion propagates")]
    [SerializeField] float _timeBeforeSpawn = 0.2f;

    //Global explosion id to ensure no explosions have the same id
    static int _NEXTID = 0;
    //Id of the explosion's chain
    int _id = 0;
    bool _idIsAssigned = false;

    [Header("Sound")]
    bool _canMakeSound = true;
    [SerializeField] SoundRequests _soundRequ;
    [SerializeField] AudioClip _clip;

    [Header("Refs")]
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _explosionObject;

    private void Start()
    {
        if (!_idIsAssigned)
        {
            _id = _NEXTID++;
        }

        //Play sound
        if (_canMakeSound)
        {
            _soundRequ.Request(_clip);
        }

        StartCoroutine(DelayedDestroy());

        StartCoroutine(SpawnExplosions());

        //Check for interactable and player to interact with
        RaycastHit2D[] hit2Ds = new RaycastHit2D[2];
        _collider.Raycast(Vector2.up, hit2Ds, 0.1f);

        //interact with everything that we overlapped
        foreach(RaycastHit2D hit2D in hit2Ds)
        {
            if (hit2D)
            {
                hit2D.collider.GetComponent<Player>()?.Kill();
                hit2D.collider.GetComponent<Interactable>()?.ExplosionInteract();
            }
        }
    }

    void AssignId(int id)
    {
        _idIsAssigned = true;
        _id = id;
    }

    bool IsExplosionAndIdDiffers(RaycastHit2D hit2D)
    {
        Explosion explo;

        //Check if hit2d has an Explosion component
        if (hit2D.collider.TryGetComponent(out explo))
        {
            //Check if id differs from our local id
            if (explo._id != _id)
            {
                return true;
            }
        }

        return false;
    }

    bool IsWallOrExplosion(RaycastHit2D hit2D)
    {
        //This is super ugly and i don't understand bitwise magic but it checks if our object's layer is
        //in the layermask
        int layerMask = LayerMask.GetMask("Walls", "Explosion");
        int layer = hit2D.collider.gameObject.layer;
        if (layerMask == (layerMask | (1 << layer)))
        {
            return true;
        }

        return false;
    }

    IEnumerator SpawnExplosions()
    {
        yield return new WaitForSeconds(_timeBeforeSpawn);

        //Check if we can propagate
        if (_power == 0)
        {
            yield break;
        }

        //Check every orthogonally adjacent tile to see if we can spawn an explosion there

        //Create direction vector
        Vector2 direction = Vector2.up;

        do
        {
            //Check if tile pointed is empty
            RaycastHit2D[] hit2Ds;
            hit2Ds = Physics2D.RaycastAll((Vector2) transform.position + direction,
                direction, 0.01f, ~LayerMask.GetMask("Water", "Ignore Raycast"));

            bool canExplode = false;
            bool canExplodeWeak = false;
            //Decide if we can explode normally
            //Can explode if we hit nothing
            if (hit2Ds.Length == 0)
                canExplode = true;
            else
            {
                //If we hit an explosion, can explode if that explosion isn't from the same chain of explosion
                if (Array.TrueForAll(hit2Ds, IsExplosionAndIdDiffers))
                {
                    canExplode = true;
                }
                //If there is something blocking us, we can still explode weakly if isn't a wall or our explosion chain
                else if (!Array.Exists(hit2Ds, IsWallOrExplosion))
                {
                    canExplodeWeak = true;
                }
            }

            //Create an explosion if conditions allows it
            if (canExplode || canExplodeWeak)
            {
                Explosion explo = Instantiate(_explosionObject, transform.position + (Vector3)direction, Quaternion.identity)
                    .GetComponent<Explosion>();

                //Make the power of that explosion to be 1 less or 0 if we have the weak explosion
                explo.Power = canExplode ? _power - 1 : 0;

                //Assign the explosion's id to be the same as this
                explo.AssignId(_id);

                //To ensure only one explosion makes sound per generation, give the new explosion the same right to make sound as this one
                explo._canMakeSound = _canMakeSound;
                //Then this explosion and it's child can't make sounds anymore
                _canMakeSound = false;
            }

            //Rotate direction vector if we aren't done
            direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90) * direction.x - Mathf.Sin(Mathf.Deg2Rad * 90) * direction.y,
                Mathf.Sin(Mathf.Deg2Rad * 90) * direction.x + Mathf.Cos(Mathf.Deg2Rad * 90) * direction.y);

        } while (direction != Vector2.up);
    }

    /// <summary>
    /// Destroy the explosion object after a time
    /// </summary>
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
