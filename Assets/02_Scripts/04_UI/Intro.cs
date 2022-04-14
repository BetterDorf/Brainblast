using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] GameObject _backgroundSprite;
    [SerializeField] int _number;

    private void Start()
    {
        for (int i = 0; i < _number; i++)
        {
            Instantiate(_backgroundSprite);
        }
    }
}
