using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    static bool _PAUSED = false;
    public static bool PAUSED { get { return _PAUSED; } }
    
    public static void PAUSE()
    {
        Time.timeScale = 0.0f;
        _PAUSED = true;
    }

    public static void UNPAUSE()
    {
        Time.timeScale = 1.0f;
        _PAUSED = false;
    }
}
