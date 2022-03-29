using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Value", menuName = "ScriptableObjects/SharedInt", order = 3)]
public class SharedIntScriptableObject : ScriptableObject
{
    private int _value = 0;
    public int Value { get { return _value; } }

    public UnityAction<int> OnValueChanged = null;

    public void ChangeValue(int newValue)
    {
        _value = newValue;

        OnValueChanged?.Invoke(_value);
    }
}
