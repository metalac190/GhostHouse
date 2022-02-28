using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.Events;

public class InteractableResponse : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;
    [SerializeField] private UnityEvent _event;

    private void OnEnable() {
        _interactable.Raise(this);
    }

    private void OnDisable() {
        _interactable.Unraise(this);
    }

    public void Invoke() {
        _event.Invoke();
    }
}