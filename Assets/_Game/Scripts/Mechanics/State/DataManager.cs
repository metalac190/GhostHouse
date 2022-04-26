using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Buttons;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;  // Singleton instance
    [SerializeField] private SceneLoader _sceneLoader = null;
    public static SceneLoader SceneLoader => Instance._sceneLoader;
    public bool _debug = false;

    private string filePath; // save file for saving & loading

    // reference to SaveData object, will hold values to save to file
    private SaveData saveData = new SaveData();

    public string level { get; set; }       // Current level season
    public int remainingSpiritPoints { get; set; }  // Current points left to spend
    public int totalUsedSpiritPoints { get; set; }  // Current points left to spend

    // Points earned towards the various endings
    public int cousinsEndingPoints { get; set; }
    public int sistersEndingPoints { get; set; }
    public int trueEndingPoints { get; set; }

    // Dictionary to hold state of each interactable
    public Dictionary<string, bool> interactions;

    // Settings options
    public bool settingsLeftClickInteract { get; set; }
    public bool settingsCameraWASD { get; set; }
    public bool settingsCameraArrowKeys { get; set; }
    public bool settingsClickDrag { get; set; }
    public int settingsSensitivity { get; set; }
    public int settingsMusicVolume { get; set; }
    public int settingsSFXVolume { get; set; }
    public int settingsDialogueVolume { get; set; }
    public int settingsAmbienceVolume { get; set; }
    public bool settingsWindowMode { get; set; }
    public int settingsContrast { get; set; }
    public int settingsBrightness { get; set; }
    public bool settingsLargeGUI { get; set; }
    public bool settingsLargeText { get; set; }
    public int settingsTextFont { get; set; }
    public bool settingsVSync { get; set; }
    public int settingsGraphicsQuality { get; set; }

    // Boolean of what has been unlocked in journal
    [HideInInspector]
    public Dictionary<string, bool> journalUnlocks;
    public bool[] endingUnlocks;

    private void Awake()
    {
        // Singleton pattern, there should only be one of these on the DataManager Prefab
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            interactions = new Dictionary<string, bool>();
            journalUnlocks = new Dictionary<string, bool>();
            endingUnlocks = new bool[4];

            saveData = new SaveData();
            filePath = Path.Combine(Application.persistentDataPath, "savedata.json");

            if (!SaveFileExists())
            {
                SetDefaultValues();
                ResetData();
                WriteFile();
            }
            else
            {
                ReadFile();

                // When Starting from a scene (not main menu), reset interactions / data
#if UNITY_EDITOR
                if (SceneManager.GetActiveScene().name.ToLower() != "mainmenu") {
                    ResetData();
                }
#endif
                if (_sceneLoader == null) {
                    _sceneLoader = GetComponentInChildren<SceneLoader>();
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnNewGame() {
        ResetData();
    }

    public void OnContinueGame() {
        // TODO: LOAD ALL INTERACTIONS FROM PREVIOUS ENDING
        if (_debug) Debug.Log("Continuing from previous save file");
        ResetData();
        ReadFile();
    }

    public Season GetSeason() {
        switch (level) {
            case "Spring":
                return Season.Spring;
            case "Summer":
                return Season.Summer;
            case "Fall":
                return Season.Fall;
            case "Winter":
                return Season.Winter;
            default:
                if (_debug) Debug.LogWarning("Season accessed on Invalid Level", gameObject);
                return Season.Universal;
        }
    }

    // Set the values to their default values at the start of Spring on a fresh save
    private void SetDefaultValues()
    {
        level = "Spring";
        remainingSpiritPoints = 3;
        trueEndingPoints = 0;
        sistersEndingPoints = 0;
        cousinsEndingPoints = 0;
        settingsLeftClickInteract = true;
        settingsCameraWASD = true;
        settingsCameraArrowKeys = true;
        settingsClickDrag = false;
        settingsSensitivity = 75;
        settingsMusicVolume = 75;
        settingsSFXVolume = 75;
        settingsDialogueVolume = 75;
        settingsAmbienceVolume = 75;
        settingsWindowMode = true;
        settingsContrast = 0;
        settingsBrightness = 0;
        settingsLargeGUI = true;
        settingsLargeText = true;
        settingsTextFont = 2;           // open dyslexic
        settingsVSync = true;
        settingsGraphicsQuality = 1;    // medium graphics
    }

    // Read data from the save file into the game
    public void ReadFile()
    {
        if (File.Exists(filePath))
        {
            // Unpack file text as JSON
            if (_debug) Debug.Log("Unpacking file into savedata");
            string fileContents = File.ReadAllText(filePath);
            saveData = new SaveData();
            JsonUtility.FromJsonOverwrite(fileContents, saveData);

            try
            {
                level = saveData.level;

                cousinsEndingPoints = saveData.cousinsEndingPoints;
                sistersEndingPoints = saveData.sistersEndingPoints;
                trueEndingPoints = saveData.trueEndingPoints;

                // Repopulate dictionary from saved arrays
                for(int i = 0; i < saveData.interactionNames.Length; i++)
                {
                    interactions[saveData.interactionNames[i]] = saveData.interactionStates[i];
                }
                interactions.Remove("");

                settingsLeftClickInteract = saveData.settings.leftClickInteract;
                settingsCameraWASD = saveData.settings.cameraWASD;
                settingsCameraArrowKeys = saveData.settings.cameraArrowKeys;
                settingsClickDrag = saveData.settings.clickDrag;
                settingsSensitivity = saveData.settings.sensitivity;
                settingsMusicVolume = saveData.settings.musicVolume;
                settingsSFXVolume = saveData.settings.sfxVolume;
                settingsDialogueVolume = saveData.settings.dialogueVolume;
                settingsAmbienceVolume = saveData.settings.ambienceVolume;
                settingsWindowMode = saveData.settings.windowMode;
                settingsContrast = saveData.settings.contrast;
                settingsBrightness = saveData.settings.brightness;
                settingsLargeGUI = saveData.settings.largeGUIFont;
                settingsLargeText = saveData.settings.largeTextFont;
                settingsTextFont = saveData.settings.textFont;
                settingsVSync = saveData.settings.vsync;
                settingsGraphicsQuality = saveData.settings.graphicsQuality;

                for (int i = 0; i < saveData.journalInteractionNames.Length; i++)
                {
                    journalUnlocks[saveData.journalInteractionNames[i]] = saveData.journalUnlocks[i];
                }
                journalUnlocks.Remove("");

                saveData.endingUnlocks.CopyTo(endingUnlocks, 0);

                if (_debug) Debug.Log("Successful read");
            }
            catch
            {
                if (_debug) Debug.Log("Some error loading save file");
                SetDefaultValues();
                ResetData();
                WriteFile();
            }
        }
        else
        {
            if (_debug) Debug.Log("No save file exists");
        }
    }

    // Write the data into the save file
    public void WriteFile()
    {
        saveData.level = level;

        saveData.cousinsEndingPoints = cousinsEndingPoints;
        saveData.sistersEndingPoints = sistersEndingPoints;
        saveData.trueEndingPoints = trueEndingPoints;

        // Unpack dictionary elements into two arrays to save
        int ind = 0;
        saveData.interactionNames = new string[160];
        saveData.interactionStates = new bool[160];
        foreach (KeyValuePair<string, bool> entry in interactions)
        {
            if(ind >= 160)
            {
                if (_debug) Debug.Log("Error: Unexpectedly high number of interactions");
            }
            else
            {
                saveData.interactionNames[ind] = entry.Key;
                saveData.interactionStates[ind] = entry.Value;
                ind++;
            }
        }
        for (int i = ind; i < 160; i++) {
            saveData.interactionNames[i] = "";
            saveData.interactionStates[i] = false;
        }

        saveData.settings.leftClickInteract = settingsLeftClickInteract;
        saveData.settings.cameraWASD = settingsCameraWASD;
        saveData.settings.cameraArrowKeys = settingsCameraArrowKeys;
        saveData.settings.clickDrag = settingsClickDrag;
        saveData.settings.sensitivity = settingsSensitivity;
        saveData.settings.musicVolume = settingsMusicVolume;
        saveData.settings.sfxVolume = settingsSFXVolume;
        saveData.settings.dialogueVolume = settingsDialogueVolume;
        saveData.settings.ambienceVolume = settingsAmbienceVolume;
        saveData.settings.windowMode = settingsWindowMode;
        saveData.settings.contrast = settingsContrast;
        saveData.settings.brightness = settingsBrightness;
        saveData.settings.largeGUIFont = settingsLargeGUI;
        saveData.settings.largeTextFont = settingsLargeText;
        saveData.settings.textFont = settingsTextFont;
        saveData.settings.vsync = settingsVSync;
        saveData.settings.graphicsQuality = settingsGraphicsQuality;

        ind = 0;
        saveData.journalInteractionNames = new string[160];
        saveData.journalUnlocks = new bool[160];
        foreach (KeyValuePair<string, bool> entry in journalUnlocks)
        {
            if (ind >= 160)
            {
                if (_debug) Debug.Log("Error: Unexpectedly high number of interactions");
            }
            else
            {
                saveData.journalInteractionNames[ind] = entry.Key;
                saveData.journalUnlocks[ind] = entry.Value;
                ind++;
            }
        }
        for (int i = ind; i < 160; i++)
        {
            saveData.journalInteractionNames[i] = "";
            saveData.journalUnlocks[i] = false;
        }

        endingUnlocks.CopyTo(saveData.endingUnlocks, 0);

        // Save data as json string and write to file
        string jsonString = JsonUtility.ToJson(saveData, true);
        if (_debug) Debug.Log("File saved to " + filePath);
        File.WriteAllText(filePath, jsonString);
    }

    public void WriteSettings()
    {
        saveData.settings.leftClickInteract = settingsLeftClickInteract;
        saveData.settings.cameraWASD = settingsCameraWASD;
        saveData.settings.cameraArrowKeys = settingsCameraArrowKeys;
        saveData.settings.clickDrag = settingsClickDrag;
        saveData.settings.sensitivity = settingsSensitivity;
        saveData.settings.musicVolume = settingsMusicVolume;
        saveData.settings.sfxVolume = settingsSFXVolume;
        saveData.settings.dialogueVolume = settingsDialogueVolume;
        saveData.settings.ambienceVolume = settingsAmbienceVolume;
        saveData.settings.windowMode = settingsWindowMode;
        saveData.settings.contrast = settingsContrast;
        saveData.settings.brightness = settingsBrightness;
        saveData.settings.largeGUIFont = settingsLargeGUI;
        saveData.settings.largeTextFont = settingsLargeText;
        saveData.settings.textFont = settingsTextFont;
        saveData.settings.vsync = settingsVSync;
        saveData.settings.graphicsQuality = settingsGraphicsQuality;

        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    // Interactables call this on their Start() to initialize themselves in the interactions dictionary
    public void SetDefaultInteraction(string name) {
        if (interactions.ContainsKey(name)) return;
        interactions.Add(name, false);
        if (!journalUnlocks.ContainsKey(name))
        {
            journalUnlocks.Add(name, false);
        }
    }

    // Set interaction state
    public void SetInteraction(string name, bool interacted)
    {
        interactions[name] = interacted;
        journalUnlocks[name] = interacted;
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
            // This shouldn't happen if interactions initialize correctly
            if (_debug) Debug.Log("Interaction not stored");
            return false;
        }
    }

    // Save settings from the control settings menu
    public void SaveControlSettings(bool leftClick, bool useWASD, bool useArrows, bool clickDrag, int sensitivity)
    {
        settingsLeftClickInteract = leftClick;
        settingsCameraWASD = useWASD;
        settingsCameraArrowKeys = useArrows;
        settingsClickDrag = clickDrag;
        settingsSensitivity = sensitivity;

        WriteSettings();
    }

    // Save settings from the audio settings menu
    public void SaveAudioSettings(int musicVol, int sfxVol, int dialogueVol, int ambVol)
    {
        settingsMusicVolume = musicVol;
        settingsSFXVolume = sfxVol;
        settingsDialogueVolume = dialogueVol;
        settingsAmbienceVolume = ambVol;

        WriteSettings();
    }

    // Save settings from the visual settings menu
    public void SaveVisualSettings(bool windowMode, int contrast, int brightness, bool largeGUIFont, bool largeTextFont, int textFont, bool vsync, int graphicsQuality)
    {
        settingsWindowMode = windowMode;
        settingsContrast = contrast;
        settingsBrightness = brightness;
        settingsLargeGUI = largeGUIFont;
        settingsLargeText = largeTextFont;
        settingsTextFont = textFont;
        settingsVSync = vsync;
        settingsGraphicsQuality = graphicsQuality;

        WriteSettings();
    }

    // Dump all data to the console
    public void DumpData()
    {
        string outstr = "Data Dump";
        outstr += "\nLevel: " + level.ToString();
        outstr += "\nSpirit Points: " + remainingSpiritPoints.ToString();
        outstr += "\nCousins Ending Points: " + cousinsEndingPoints.ToString();
        outstr += "\nSisters Ending Points: " + sistersEndingPoints.ToString();
        outstr += "\nTrue Ending Points: " + trueEndingPoints.ToString();
        outstr += "\nInteractables:";
        foreach(KeyValuePair<string, bool> entry in interactions)
        {
            outstr += "\n\tInteractable " + entry.Key + ": " + entry.Value;
        }
        outstr += "\nSettings:";
        outstr += "\n\tLeft Click Interact: " + settingsLeftClickInteract.ToString();
        outstr += "\n\tWASD Camera Use: " + settingsCameraWASD.ToString();
        outstr += "\n\tArrow Key Camera Use: " + settingsCameraArrowKeys.ToString();
        outstr += "\n\tClick and Drag Camera: " + settingsClickDrag.ToString();
        outstr += "\n\tSensitivity: " + settingsSensitivity.ToString();
        outstr += "\n\tMusic Volume: " + settingsMusicVolume.ToString();
        outstr += "\n\tSFX Volume: " + settingsSFXVolume.ToString();
        outstr += "\n\tDialogue Volume: " + settingsDialogueVolume.ToString();
        outstr += "\n\tAmbience Volume: " + settingsAmbienceVolume.ToString();
        outstr += "\n\tWindow Mode: " + settingsMusicVolume.ToString();
        outstr += "\n\tContrast: " + settingsContrast.ToString();
        outstr += "\n\tBrightness: " + settingsBrightness.ToString();
        outstr += "\n\tLarge GUI Font: " + settingsLargeGUI.ToString();
        outstr += "\n\tLarge Text Font: " + settingsLargeText.ToString();
        outstr += "\n\tText Font Style: " + settingsTextFont.ToString();
        outstr += "\n\tVSync: " + settingsVSync.ToString();
        outstr += "\n\tGraphics Quality: " + settingsGraphicsQuality.ToString();
        outstr += "\nJournal Unlocks: ";
        foreach (KeyValuePair<string, bool> entry in journalUnlocks)
        {
            outstr += "\n\tJournal Entry " + entry.Key + ": " + entry.Value;
        }
        outstr += "\nEnding Unlocks: ";
        for (int i = 0; i < endingUnlocks.Length; i++)
        {
            if (endingUnlocks[i])
            {
                outstr += i.ToString() + " ";
            }
        }
        if (_debug) Debug.Log(outstr);
    }

    // Dump save file contents to the console
    public void DumpFileContents()
    {
        if (_debug) Debug.Log(File.ReadAllText(filePath));
    }

    public void UnlockEnding(int index)
    {
        endingUnlocks[index] = true;
        WriteFile();
    }

    // For now, just resets interactions. Will need to clear save file eventually
    [Button(Mode = ButtonMode.NotPlaying)]
    public void ResetData()
    {
        interactions.Clear();
        cousinsEndingPoints = 0;
        sistersEndingPoints = 0;
        trueEndingPoints = 0;
    }

    public void ResetAllData()
    {
        interactions.Clear();
        journalUnlocks.Clear();
        level = "Spring";
        remainingSpiritPoints = 3;
        trueEndingPoints = 0;
        sistersEndingPoints = 0;
        cousinsEndingPoints = 0;
        WriteFile();
    }

    // Return whether or not the save file exists
    public bool SaveFileExists()
    {
        return System.IO.File.Exists(filePath);
    }
}