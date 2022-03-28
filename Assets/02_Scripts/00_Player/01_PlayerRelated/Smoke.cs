using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public void OnAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
