using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    /*Here it is!! The big class, itself. I am looking into adding a WhileHovering Method, but for now, I am happy with these four methods. As long as
     design is happy with these, there isn't too much I need to do.*/
    void OnLeftClick(Vector3 mousePosition);
    void OnRightClick(Vector3 mousePosition);
    void OnLeftClick();
    void OnRightClick();

    void OnHoverEnter();
    void OnHoverExit();
}
