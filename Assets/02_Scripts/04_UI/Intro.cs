using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private void Start()
    {
        GameManager.PAUSE();
    }

    public void ContinueButton()
    {
        GameManager.UNPAUSE();
        gameObject.SetActive(false);
    }
}
