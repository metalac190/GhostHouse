using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int level; // Store level season as int
    public int remainingSpiritPoints; // Store spirit points remaining from last save

    // Array of interactions to save
    public string[] interactionNames;
    public bool[] interactionStates;

    // Serializable Dialogue struct to keep track of dialogue progress
    /*[System.Serializable]
    public struct Dialogue
    {

    }
    // Array of dialogue progress to save
    public Dialogue[] dialogues;*/

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
    }
    public Settings settings;

    // Boolean array of journal unlocks
    public bool[] journalUnlocks;

    // Constructor to initialize arrays
    public SaveData()
    {
        interactionNames = new string[50];
        interactionStates = new bool[50];
        journalUnlocks = new bool[50];
    }
}