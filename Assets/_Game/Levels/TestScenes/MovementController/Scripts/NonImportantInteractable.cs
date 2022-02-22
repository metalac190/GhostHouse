using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonImportantInteractable : InteractableBase
{
    [SerializeField] GameObject _particleEffect = null;

    

    public override void OnLeftClick(Vector3 mousePosition)
    {
        Instantiate(_particleEffect, mousePosition, Quaternion.identity);
        Debug.Log("Left Clicked On: " + gameObject.name + " and Location at: " + (mousePosition));
        
    }
}
