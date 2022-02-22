using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonImportantInteractable : InteractableBase
{
    [SerializeField] GameObject _particleEffect = null;

    

    public override void OnLeftClick()
    {
        Instantiate(_particleEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        Debug.Log("Left Clicked On: " + gameObject.name + " and Location at: " + (Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        
    }
}
