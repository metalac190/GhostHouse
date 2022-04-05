using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Buttons;
using Utility.Audio.Managers;

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

    // Lazy load the Camera Controller
    private IsometricCameraController cameraController;
    private IsometricCameraController CameraController
    {
        get
        {
            if(cameraController == null) cameraController = FindObjectOfType<IsometricCameraController>();
            return cameraController;
        }
    }

    // Reference to AudioMixerController to control volume levels
    AudioMixerController audioMixerController = null;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            audioMixerController = GetComponent<AudioMixerController>();
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    //Load all settings when game starts
    private void Start() {
        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(DataManager.Instance.settingsLeftClickInteract);

        LoadSettings();
    }

    [Button(Spacing = 25, Mode = ButtonMode.NotPlaying)]
    public void LoadSettings() {
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

    [Button(Spacing = 20, Mode = ButtonMode.NotPlaying)]
    public void SaveAllSettings() {
        SaveControlSettings();
        SaveAudioSettings();
        SaveVisualSettings();
    }

    [Button(Spacing = 10, Mode = ButtonMode.NotPlaying)]
    public void SaveControlSettings() {
        DataManager.Instance.SaveControlSettings(leftClickInteract, useWASD, useArrowKeys, useClickNDrag, dragSpeed);
        SetControlSettings();
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void SaveAudioSettings() {
        DataManager.Instance.SaveAudioSettings(music, SFX, dialog, ambience);
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void SaveVisualSettings() {
        DataManager.Instance.SaveVisualSettings(isWindowed, contrast, brightness, largeGUIFont, largeTextFont, textFont);
    }

    // Update the camera controller with the new settings
    private void SetControlSettings()
    {
        // Set Control settings on camera controller
        if(CameraController == null) return;
        CameraController._enableWASDMovement = useWASD;
        CameraController._enableClickDragMovement = useClickNDrag;
    }

    // Update audio mixer controller with audio values
    private void SetAudioSettings()
    {
        // Assuming 0 to 100 instead of 0 to 1
        audioMixerController.SetMusicVolume(music * 0.01f);
        audioMixerController.SetSfxVolume(SFX * 0.01f);
        audioMixerController.SetDialogueVolume(dialog * 0.01f);
        audioMixerController.SetAmbienceVolume(ambience * 0.01f);
    }

    // Update visual settings in the font manager and elsewhere
    private void SetVisualSettings()
    {
        // set font
        FontManager fontManager = FontManager.Instance;
        fontManager.UpdateAllText((FontMode)textFont);

        // set post-processing volume
        GraphicsController.ScreenMode = isWindowed ? FullScreenMode.FullScreenWindow : FullScreenMode.ExclusiveFullScreen;
        GraphicsController.Exposure = brightness;
        GraphicsController.Contrast = contrast;
    }
}