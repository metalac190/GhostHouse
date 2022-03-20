using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Load all settings when game starts
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

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

    public void SaveControlSettings()
    {
        DataManager.Instance.SaveControlSettings(leftClickInteract, useWASD, useArrowKeys, useClickNDrag, dragSpeed);
    }

    public void SaveAudioSettings()
    {
        DataManager.Instance.SaveAudioSettings(music, SFX, dialog, ambience);
    }

    public void SaveVisualSettings()
    {
        DataManager.Instance.SaveVisualSettings(isWindowed, contrast, brightness, largeGUIFont, largeTextFont, textFont);
    }
}
