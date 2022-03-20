using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/Event", order = 2)]
public class EventScriptableObject : ScriptableObject
{
    public UnityAction OnEventTriggered = null;
    
    public void TriggerEvent()
    {
        OnEventTriggered?.Invoke();
    }
}
