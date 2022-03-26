using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneVat : Interactable
{
    [SerializeField] ObjectReferenceScriptableObject _selectedCloneVatReference;
    bool _isSelected = false;

    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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
        _animator.SetBool("isOn", _isSelected);

        //Play a sound
        GetComponent<AudioSource>()?.Play();

        return true;
    }

    void DeSelect()
    {
        _selectedCloneVatReference.OnReferenceChanged -= DeSelect;
        _isSelected = false;

        //Cosmetic change
        _animator.SetBool("isOn", _isSelected);
    }

    private void OnDestroy()
    {
        _selectedCloneVatReference.OnReferenceChanged -= DeSelect;
    }
}
