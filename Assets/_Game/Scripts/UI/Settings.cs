using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Buttons;

//Settings menus change settings values
public class Settings : MonoBehaviour
{

    //Singleton pattern
    public static Settings Instance = null;

    //Interact Button
    public bool leftClickInteract = true;

    //Camera Movement
    public bool useWASD = true;
    public bool useArrowKeys = true;
    public bool useClickNDrag = false;
    public int dragSpeed = 75;

    //Audio Settings
    public int music = 100;
    public int SFX = 100;
    public int dialog = 100;
    public int ambience = 100;

    //Visual Settings
    public bool isWindowed = false;
    public int contrast = 75;
    public int brightness = 75;
    public bool largeGUIFont = false;
    public bool largeTextFont = false;

    //0 - Fancy, 1 - Normal, 2 - Dyslexia Friendly
    [Range(0, 2)]
    public int textFont = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //Load all settings when game starts
    private void Start()
    {
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(DataManager.Instance.settingsLeftClickInteract);

        LoadSettings();
    }

    [Button(Spacing = 25)]
    public void LoadSettings()
    {
        // This should be called each time the settings menu is opened

        leftClickInteract = DataManager.Instance.settingsLeftClickInteract;
        useWASD = DataManager.Instance.settingsCameraWASD;
        useArrowKeys = DataManager.Instance.settingsCameraArrowKeys;
        useClickNDrag = DataManager.Instance.settingsClickDrag;
        dragSpeed = DataManager.Instance.settingsSensitivity;
        music = DataManager.Instance.settingsMusicVolume;
        SFX = DataManager.Instance.settingsSFXVolume;
        dialog = DataManager.Instance.settingsDialogueVolume;
        ambience = DataManager.Instance.settingsAmbienceVolume;
        isWindowed = DataManager.Instance.settingsWindowMode;
        contrast = DataManager.Instance.settingsContrast;
        brightness = DataManager.Instance.settingsBrightness;
        largeGUIFont = DataManager.Instance.settingsLargeGUI;
        largeTextFont = DataManager.Instance.settingsLargeText;
        textFont = DataManager.Instance.settingsTextFont;
    }

    [Button(Spacing = 20)]
    public void SaveAllSettings() {
        SaveControlSettings();
        SaveAudioSettings();
        SaveVisualSettings();
    }

    [Button(Spacing = 10)]
    public void SaveControlSettings()
    {
        DataManager.Instance.SaveControlSettings(leftClickInteract, useWASD, useArrowKeys, useClickNDrag, dragSpeed);
    }

    [Button]
    public void SaveAudioSettings()
    {
        DataManager.Instance.SaveAudioSettings(music, SFX, dialog, ambience);
    }

    [Button]
    public void SaveVisualSettings()
    {
        DataManager.Instance.SaveVisualSettings(isWindowed, contrast, brightness, largeGUIFont, largeTextFont, textFont);
    }
}
