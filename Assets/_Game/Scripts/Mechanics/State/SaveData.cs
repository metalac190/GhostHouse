using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public string level; // Store level season

    // Store the points towards the various endings
    public int cousinsEndingPoints;
    public int sistersEndingPoints;
    public int trueEndingPoints;

    // Array of interactions to save
    public string[] interactionNames;
    public bool[] interactionStates;

    // Serializable Settings struct
    [System.Serializable]
    public struct Settings
    {
        public bool leftClickInteract;
        public bool cameraWASD;
        public bool cameraArrowKeys;
        public bool clickDrag;
        public int sensitivity;
        public int musicVolume;
        public int sfxVolume;
        public int dialogueVolume;
        public int ambienceVolume;
        public bool windowMode;
        public int contrast;
        public int brightness;
        public bool largeGUIFont;
        public bool largeTextFont;
        public int textFont;
        public bool vsync;
        public int graphicsQuality;
    }
    public Settings settings;

    // Boolean array of journal unlocks
    public string[] journalInteractionNames;
    public bool[] journalUnlocks;

    // Constructor to initialize arrays
    public SaveData()
    {
        interactionNames = new string[200];
        interactionStates = new bool[200];
        journalInteractionNames = new string[200];
        journalUnlocks = new bool[200];
    }
}