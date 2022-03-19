using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Local storage of settings data - talks to DataManager for permanent storage
public class Settings : MonoBehaviour
{
    //Interact Button
    bool leftClickInteract = true;

    //Camera Movement
    bool useWASD = true;
    bool useArrowKeys = true;
    bool useClickNDrag = false;
    int dragSpeed = 75;

    //Audio Settings
    int music = 100;
    int SFX = 100;
    int dialog = 100;
    int ambience = 100;

    //Visual Settings
    bool isWindowed = false;
    int contrast = 75;
    int brightness = 75;
    bool largeGUIFont = false;
    bool largeTextFont = false;

    //0 - Fancy, 1 - Normal, 2 - Dyslexia Friendly
    [Range(0, 2)]
    int textFont = 0;

    //Panels and Menu Components

    //Load from data manager
    private void Awake()
    {

    }

    //Save to data manager
    public void Save()
    {

    }

    //Update data
    private void Update()
    {

    }


}
