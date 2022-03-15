using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectReferenceScriptableObject", order = 1)]
public class ObjectReferenceScriptableObject : ScriptableObject
{
    public delegate void ReferenceChangedEvent();
    //Objects that add themselves have the moral obligation to guarantee to remove themselves
    public ReferenceChangedEvent OnReferenceChanged;

    //The shared reference to a gameObject
    GameObject _gameObject;
    public GameObject GameObject { get => _gameObject; }

    public GameObject ChangeGameObject(GameObject gameObject)
    {
        _gameObject = gameObject;

        OnReferenceChanged?.Invoke();

        return gameObject;
    }
}
