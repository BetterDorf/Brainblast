using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactable
{
    [Tooltip("Breaking Sounds")]
    [SerializeField] List<AudioClip> _clips;
    [SerializeField] SoundRequests _sound;

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

        //Play sound
        _sound.Request(_clips[Random.Range(0, _clips.Count)]);

        Destroy(gameObject);
    }
}
