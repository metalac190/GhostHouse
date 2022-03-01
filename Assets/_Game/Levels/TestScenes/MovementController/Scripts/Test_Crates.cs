using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Crates : InteractableBase
{
    /*This is a test class that I created to show case how an interactable would work. Obviously, there will probably be an interactable base 
     class, but that class will still inherit the IInteractable Interface. For this, I kinda just skipped the interactable base class and just made
     and interactable crate.*/

    //This is not required. This is just to showcase visually how the IInteractable methods would work.
    [SerializeField] Rigidbody _rigidbody = null;
    [SerializeField] Renderer _meshRenderer = null;
    [SerializeField] Material _highlightMaterial = null;
    [SerializeField] Material _normalMaterial = null;

    //These are just sample visual feedback methods.
    #region VisualFeedback
    //void MoveHorizontal()
    //{
    //    Quaternion turnOffset = Quaternion.Euler(0, 30f, 0);
    //    if (_rigidbody != null)
    //    {
    //        _rigidbody.MoveRotation(_rigidbody.rotation * turnOffset);
    //    }
    //}

    void MoveVerticalBackwards()
    {
        Quaternion turnOffset = Quaternion.Euler(-30f, 0, -20f);
        if (_rigidbody != null)
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * turnOffset);
        }
    }

    void MoveVerticalForwards()
    {
        Quaternion turnOffset = Quaternion.Euler(30f, 0, 20f);
        if (_rigidbody != null)
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * turnOffset);
        }
    }
    #endregion

    //This is when the mouse first hovers over the object.
    //public override void OnHoverEnter()
    //{
    //    Debug.Log("Hovering over " + gameObject.name);
    //    //MoveHorizontal();
    //    if (_meshRenderer != null) _meshRenderer.material = _highlightMaterial;
    //}

    ////This is when the mouse leaves the shape of the object.
    //public override void OnHoverExit()
    //{
    //    Debug.Log("No Longer Hovering over" + gameObject.name);
    //    if (_meshRenderer != null) _meshRenderer.material = _normalMaterial;
    //}

    //This is when the mouse left clicks on the object.
    public override void OnLeftClick()
    {
        Debug.Log("Left Clicked On" + gameObject.name);
        MoveVerticalForwards();
    }

    //This is when the mouse right clicks on an object.
    public override void OnRightClick()
    {
        Debug.Log("Right Clicked On" + gameObject.name);
        MoveVerticalBackwards();
    }




}
