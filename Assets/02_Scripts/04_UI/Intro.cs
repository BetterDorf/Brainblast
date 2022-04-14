using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] GameObject _backgroundSprite;
    [SerializeField] int _number;

    [SerializeField] bool _quitOnKeyDown = false;

    private void Start()
    {
        for (int i = 0; i < _number; i++)
        {
            Instantiate(_backgroundSprite);
        }
    }

    private void Update()
    {
        //Exit this screen when anything is pressed
        if (Input.anyKeyDown)
        {
            //Quit the game if we're the last scene
            if (_quitOnKeyDown)
            {
                Application.Quit();
            }
            else
            {
                //load the next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
