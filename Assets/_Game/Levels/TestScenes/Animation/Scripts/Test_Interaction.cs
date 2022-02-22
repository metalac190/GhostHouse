using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test_Interaction : MonoBehaviour, IInteractable
{
    //generic name for futher implimentation
    public event Action InvokeAction;

    public virtual void OnHoverEnter()
    {
        //do nothing
    }
    public virtual void OnHoverExit()
    {
        //do nothing
    }
    public virtual void OnLeftClick()
    {
        Debug.Log("Clicked Object");
        InvokeAction?.Invoke();
    }
    public virtual void OnRightClick()
    {
        //do nothing
    }

   
}
