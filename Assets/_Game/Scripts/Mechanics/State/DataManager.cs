using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility.Buttons;

public class DataManager : MonoBehaviour
{
    // public instance reference to this script
    public static DataManager Instance = null;

    // string to hold the path to the savefile
    private string filePath;

    // rweference to SaveData object, will hold values to save
    private SaveData saveData = new SaveData();

    // Save Data fields saved in DataManager
    public int level { get; set; }
    public int remainingSpiritPoints { get; set; }

    // Dictionary to hold state of each interactable
    public Dictionary<string, bool> interactions;

    // Settings options
    public float settingsSensitivity { get; set; }
    public float settingsMusicVolume { get; set; }
    public float settingsSFXVolume { get; set; }
    public float settingsDialogueVolume { get; set; }
    public float settingsAmbienceVolume { get; set; }
    public float settingsBrightness { get; set; }
    public int settingsWindowMode { get; set; }

    // Boolean of what has been unlocked in journal
    [HideInInspector]
    public bool[] journalUnlocks;

    private void Awake()
    {
        // Singleton pattern, there should only be one of these on the DataManager Prefab
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            filePath = Path.Combine(Application.persistentDataPath, "savedata.json");
            journalUnlocks = new bool[10];
            interactions = new Dictionary<string, bool>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Read data from the save file into the game
    public void ReadFile()
    {
        if (File.Exists(filePath))
        {
            // Unpack file text as JSON
            string fileContents = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(fileContents, saveData);

            level = saveData.level;
            remainingSpiritPoints = saveData.remainingSpiritPoints;

            // Repopulate dictionary from saved arrays
            for(int i = 0; i < 50; i++)
            {
                interactions[saveData.interactionNames[i]] = saveData.interactionStates[i];
            }

            settingsSensitivity = saveData.settings.sensitivity;
            settingsMusicVolume = saveData.settings.musicVolume;
            settingsSFXVolume = saveData.settings.sfxVolume;
            settingsDialogueVolume = saveData.settings.dialogueVolume;
            settingsAmbienceVolume = saveData.settings.ambienceVolume;
            settingsBrightness = saveData.settings.brightness;
            settingsWindowMode = saveData.settings.windowMode;

            saveData.journalUnlocks.CopyTo(journalUnlocks, 0);
        }
        else
        {
            Debug.Log("No save file exists");
        }
    }

    // Write the data into the save file
    public void WriteFile()
    {
        saveData.level = level;
        saveData.remainingSpiritPoints = remainingSpiritPoints;

        // Unpack dictionary elements into two arrays to save
        foreach(KeyValuePair<string, bool> entry in interactions)
        {
            int i = 0;
            saveData.interactionNames[i] = entry.Key;
            saveData.interactionStates[i] = entry.Value;
            i++;
        }

        saveData.settings.sensitivity = settingsSensitivity;
        saveData.settings.musicVolume = settingsMusicVolume;
        saveData.settings.sfxVolume = settingsSFXVolume;
        saveData.settings.dialogueVolume = settingsDialogueVolume;
        saveData.settings.ambienceVolume = settingsAmbienceVolume;
        saveData.settings.brightness = settingsBrightness;
        saveData.settings.windowMode = settingsWindowMode;

        journalUnlocks.CopyTo(saveData.journalUnlocks, 0);

        // Save data as json string and write to file
        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    // Set interaction state
    public void SetInteraction(string name, bool interacted)
    {
        interactions[name] = interacted;
    }

    // Get interaction state of an interaction
    public bool GetInteraction(string name)
    {
        if (interactions.ContainsKey(name))
        {
            return interactions[name];
        }
        else
        {
            Debug.Log("Interaction not stored");
            return false;
        }
    }

    // Dump all data to the console
    public void DumpData()
    {
        string outstr = "Data Dump";
        outstr += "\nLevel: " + level.ToString();
        outstr += "\nSpirit Points: " + remainingSpiritPoints.ToString();
        outstr += "\nInteractables:";
        foreach(KeyValuePair<string, bool> entry in interactions)
        {
            outstr += "\n\tInteractable " + entry.Key + ": " + entry.Value;
        }
        outstr += "\nSettings:";
        outstr += "\n\tSensitivity: " + settingsSensitivity.ToString();
        outstr += "\n\tMusic Volume: " + settingsMusicVolume.ToString();
        outstr += "\n\tSFX Volume: " + settingsSFXVolume.ToString();
        outstr += "\n\tDialogue Volume: " + settingsDialogueVolume.ToString();
        outstr += "\n\tAmbience Volume: " + settingsAmbienceVolume.ToString();
        outstr += "\n\tBrightness: " + settingsBrightness.ToString();
        outstr += "\n\tWindow Mode: " + settingsMusicVolume.ToString();
        outstr += "\nJournal Unlocks: ";
        for (int i = 0; i < journalUnlocks.Length; i++) {
            if (journalUnlocks[i]) {
                outstr += i.ToString() + " ";
            }
        }
        Debug.Log(outstr);
    }

    // Dump save file contents to the console
    public void DumpFileContents()
    {
        Debug.Log(File.ReadAllText(filePath));
    }

    // Mark a journal entry as unlocked
    public void UnlockJournalEntry(int index)
    {
        journalUnlocks[index] = true;
    }

    // For now, just resets interactions. Will need to clear save file eventually
    [Button(Mode = ButtonMode.NotPlaying)]
    public void ResetData()
    {
        interactions.Clear();
    }
}