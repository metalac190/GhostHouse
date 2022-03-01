using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.Events;

public class InteractableResponse : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private UnityEvent _event = new UnityEvent();

    private bool _ignore;

    private void OnEnable() {
        if (_interactable == null) return;
        _interactable.Raise(this);
    }

    private void Start() {
        if (_interactable == null) return;
        _interactable.LoadInteraction();
        if (!_interactable.CanInteract) {
            _event.Invoke();
            _ignore = true;
        }
        else {
            _ignore = false;
        }
    }

    private void OnDisable() {
        if (_interactable == null) return;
        _interactable.Unraise(this);
    }

    public void Invoke() {
        if (_ignore) return;
        _event.Invoke();
    }
}