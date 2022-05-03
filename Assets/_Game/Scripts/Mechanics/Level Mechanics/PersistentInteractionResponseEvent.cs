using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.Level_Mechanics;
using UnityEngine.Events;

public class PersistentInteractionResponseEvent : MonoBehaviour
{
    [SerializeField] private Interactable interactable = null;
    [SerializeField] private UnityEvent _event = new UnityEvent();


    public void Start()
    {
        if (interactable != null && interactable.Interacted)
        {
            _event.Invoke();
        }
    }
}
