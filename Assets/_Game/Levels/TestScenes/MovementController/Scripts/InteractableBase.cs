using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    public virtual void OnHoverEnter()
    {
        
    }
    public virtual void OnHoverExit()
    {
        
    }
    public virtual void OnLeftClick()
    {
        
    }
    public virtual void OnRightClick()
    {
        
    }

    public virtual void OnLeftClick(Vector3 mousePosition)
    {

    }
    public virtual void OnRightClick(Vector3 mousePosition)
    {

    }


}
