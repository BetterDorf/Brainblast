using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneVat : Interactable
{
    [SerializeField] ObjectReferenceScriptableObject _selectedCloneVatReference;
    bool _isSelected = false;

    private void Start()
    {
        if (!_isSelected)
            DeSelect();
    }

    public override bool Interact()
    {
        return Select();
    }

    public bool Select()
    {
        if (_isSelected)
        {
            return false;
        }

        _isSelected = true;
        _selectedCloneVatReference.ChangeGameObject(gameObject);
        _selectedCloneVatReference.OnReferenceChanged += DeSelect;

        //Cosmetic change
        GetComponent<SpriteRenderer>().color = Color.white;

        //Play a sound
        GetComponent<AudioSource>()?.Play();

        return true;
    }

    void DeSelect()
    {
        _selectedCloneVatReference.OnReferenceChanged -= DeSelect;
        _isSelected = false;

        //Cosmetic change
        GetComponent<SpriteRenderer>().color = Color.grey;
    }

    private void OnDestroy()
    {
        _selectedCloneVatReference.OnReferenceChanged -= DeSelect;
    }
}
