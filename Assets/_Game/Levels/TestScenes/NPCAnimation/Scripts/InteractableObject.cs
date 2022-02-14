using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {

        NPCBase _npc;
        Animator _animator;

        void Awake()
        {
            _npc = FindObjectOfType<NPCBase>();
            _animator = _npc.GetComponent<Animator>();
        }

        //This is when the mouse first hovers over the object.
        public void OnHoverEnter()
        {

        }

        //This is when the mouse leaves the shape of the object.
        public void OnHoverExit()
        {

        }

        //This is when the mouse left clicks on the object.
        public void OnLeftClick()
        {
            _animator.SetTrigger("TriggerAngry");
        }

        //This is when the mouse right clicks on an object.
        public void OnRightClick()
        {
        }
    }
}
