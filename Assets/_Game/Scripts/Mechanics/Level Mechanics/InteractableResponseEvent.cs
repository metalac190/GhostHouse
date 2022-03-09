using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.Events;

public class InteractableResponseEvent : InteractableResponseBase
{
    [SerializeField] private UnityEvent _event = new UnityEvent();

    public override void Invoke() {
        _event.Invoke();
    }
}