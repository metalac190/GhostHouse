using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public static PanelController Singleton;

    private void Awake() {
        Singleton = this;
    }

    public void EnableModalWindow(Interactable interactable, Interactable altInteractable) {
        // Carlos here

        // if (interactable != null) {
        //  Disable button
        // else
        //  Enable button and connect
    }
}