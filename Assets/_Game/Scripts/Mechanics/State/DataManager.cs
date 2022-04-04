using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility.Buttons;
using Utility.Audio.Managers;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;  // Singleton instance

    // Reference to AudioMixerController to control volume levels
    [SerializeField] AudioMixerController audioMixerController = null;

    // Lazy load the Camera Controller
    private IsometricCameraController cameraController;
    private IsometricCameraController CameraController {
        get {
            if (cameraController == null) cameraController = FindObjectOfType<IsometricCameraController>();
            return cameraController;
        }
    }

    private string filePath; // save file for saving & loading

    // reference to SaveData object, will hold values to save to file
    private SaveData saveData = new SaveData();

    public string level { get; set; }       // Current level season
    public int remainingSpiritPoints { get; set; }  // Current points left to spend

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

    // Boolean of what has been unlocked in journal
    [HideInInspector]
    public bool[] journalUnlocks;

    private void Awake()
    {
        // Singleton pattern, there should only be one of these on the DataManager Prefab
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            interactions = new Dictionary<string, bool>();
            journalUnlocks = new bool[24];      // Initializes array of all false entries

            filePath = Path.Combine(Application.persistentDataPath, "savedata.json");

            // Load all file information in Awake so other game-objects can call it in Start.
            SetDefaultValues();
            LoadFile();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Set the values to their default values at the start of Spring on a fresh save
    private void SetDefaultValues()
    {
        level = "Spring";
        remainingSpiritPoints = 3;
        cousinsEndingPoints = 0;
        sistersEndingPoints = 0;
        trueEndingPoints = 0;
        settingsLeftClickInteract = true;
        settingsCameraWASD = true;
        settingsCameraArrowKeys = true;
        settingsClickDrag = false;
        settingsSensitivity = 75;
        settingsMusicVolume = 100;
        settingsSFXVolume = 75;
        settingsDialogueVolume = 75;
        settingsAmbienceVolume = 75;
        settingsWindowMode = true;
        settingsContrast = -20;
        settingsBrightness = 0;
        settingsLargeGUI = true;    // placeholder
        settingsLargeText = true;   // placeholder
        settingsTextFont = 0;
    }

    // Load the game from file and set up the game
    private void LoadFile()
    {
        ReadFile();

        // Set all loaded settings for the player
        SetControlSettings();
        SetAudioSettings();
        SetVisualSettings();
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

            cousinsEndingPoints = saveData.cousinsEndingPoints;
            sistersEndingPoints = saveData.sistersEndingPoints;
            trueEndingPoints = saveData.trueEndingPoints;

            // Repopulate dictionary from saved arrays
            for(int i = 0; i < 48; i++)
            {
                interactions[saveData.interactionNames[i]] = saveData.interactionStates[i];
            }

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

            try
            {
                saveData.journalUnlocks.CopyTo(journalUnlocks, 0);
            }
            catch
            {
                for(int i = 0; i < journalUnlocks.Length; i++)
                {
                    journalUnlocks[i] = saveData.journalUnlocks[i];
                }
            }
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

        saveData.cousinsEndingPoints = cousinsEndingPoints;
        saveData.sistersEndingPoints = sistersEndingPoints;
        saveData.trueEndingPoints = trueEndingPoints;

        // Unpack dictionary elements into two arrays to save
        foreach(KeyValuePair<string, bool> entry in interactions)
        {
            int i = 0;
            if(i >= 48)
            {
                Debug.Log("Error: Unexpectedly high number of interactions");
            }
            else
            {
                saveData.interactionNames[i] = entry.Key;
                saveData.interactionStates[i] = entry.Value;
                i++;
            }
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

        journalUnlocks.CopyTo(saveData.journalUnlocks, 0);

        // Save data as json string and write to file
        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    // Interactables call this on their Start() to initialize themselves in the interactions dictionary
    public void SetDefaultInteraction(string name) {
        if (interactions.ContainsKey(name)) return;
        interactions.Add(name, false);
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
            // This shouldn't happen if interactions initialize correctly
            Debug.Log("Interaction not stored");
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

        SetControlSettings();
    }

    // Update the camera controller with the new settings
    private void SetControlSettings()
    {
        // Set Control settings on camera controller
        if (CameraController == null) return;
        CameraController._enableWASDMovement = settingsCameraWASD;
        CameraController._enableClickDragMovement = settingsClickDrag;

        //if (settingsCameraWASD) { CameraController._cameraMode = CameraMode.KEYBOARD; }
        //else if (settingsClickDrag) { CameraController._cameraMode = CameraMode.CLICKDRAG; }
    }

    // Save settings from the audio settings menu
    public void SaveAudioSettings(int musicVol, int sfxVol, int dialogueVol, int ambVol)
    {
        settingsMusicVolume = musicVol;
        settingsSFXVolume = sfxVol;
        settingsDialogueVolume = dialogueVol;
        settingsAmbienceVolume = ambVol;

        SetAudioSettings();
    }

    // Update audio mixer controller with audio values
    private void SetAudioSettings()
    {
        // Assuming 0 to 100 instead of 0 to 1
        audioMixerController.SetMusicVolume(settingsMusicVolume * 0.01f);
        audioMixerController.SetSfxVolume(settingsSFXVolume * 0.01f);
        audioMixerController.SetDialogueVolume(settingsDialogueVolume * 0.01f);
        audioMixerController.SetAmbienceVolume(settingsAmbienceVolume * 0.01f);
    }

    // Save settings from the visual settings menu
    public void SaveVisualSettings(bool windowMode, int contrast, int brightness, bool largeGUIFont, bool largeTextFont, int textFont)
    {
        settingsWindowMode = windowMode;
        settingsContrast = contrast;
        settingsBrightness = brightness;
        settingsLargeGUI = largeGUIFont;
        settingsLargeText = largeTextFont;
        settingsTextFont = textFont;

        SetVisualSettings();
    }

    // Update visual settings in the font manager and elsewhere
    private void SetVisualSettings()
    {
        // set font
        FontManager fontManager = FontManager.Instance;
        fontManager.UpdateAllText((FontMode) settingsTextFont);

        // set post-processing volume
        GraphicsController.ScreenMode = settingsWindowMode ? FullScreenMode.FullScreenWindow : FullScreenMode.ExclusiveFullScreen;
        GraphicsController.Exposure = settingsBrightness;
        GraphicsController.Contrast = settingsContrast;
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