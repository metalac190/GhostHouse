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
        public float sensitivity;
        public float musicVolume;
        public float sfxVolume;
        public float dialogueVolume;
        public float ambienceVolume;
        public float brightness;
        public int windowMode;
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