using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Communication between the Settings UI and Settings singleton
public class PanelMediator : MonoBehaviour
{
    [SerializeField] private bool SaveOnChange = true; 

    // References to UI values
    // Automatically hooks up connections

    [Header("Visual")]
    [SerializeField] Button FullscreenButton = null;
    [SerializeField] Button WindowedButton = null;
    [SerializeField] Slider ContrastSlider = null;
    [SerializeField] TextMeshProUGUI ContrastLabel = null;
    [SerializeField] Slider BrightnessSlider = null;
    [SerializeField] TextMeshProUGUI BrightnessLabel = null;

    [Header("Audio")]
    [SerializeField] Slider MusicSlider = null;
    [SerializeField] TextMeshProUGUI MusicLabel = null;
    [SerializeField] Slider SFXSlider = null;
    [SerializeField] TextMeshProUGUI SFXLabel = null;
    [SerializeField] Slider DialogSlider = null;
    [SerializeField] TextMeshProUGUI DialogLabel = null;
    [SerializeField] Slider AmbienceSlider = null;
    [SerializeField] TextMeshProUGUI AmbienceLabel = null;

    [Header("Font")]
    [SerializeField] Button FancyFontButton = null;
    [SerializeField] Button NormalFontButton = null;
    [SerializeField] Button DyslexiaFontButton = null;

    [Header("CameraMovement")]
    [SerializeField] Button ClickDragOn = null;
    [SerializeField] Button ClickDragOff = null;

    // Update All Values from Settings
    private void OnEnable()
    {
        SetWindowed(Settings.Instance.isWindowed);

        ContrastSlider.value = Settings.Instance.contrast;
        BrightnessSlider.value = Settings.Instance.brightness;

        MusicSlider.value = Settings.Instance.music;
        SFXSlider.value = Settings.Instance.SFX;
        DialogSlider.value = Settings.Instance.dialog;
        AmbienceSlider.value = Settings.Instance.ambience;
    }

    // Add Listeners to update settings when changed
    private void Start() {
        FullscreenButton.onClick.AddListener(SetFullscreen);
        WindowedButton.onClick.AddListener(SetWindowed);

        ContrastSlider.onValueChanged.AddListener(ChangeContrast);
        BrightnessSlider.onValueChanged.AddListener(ChangeBrightness);

        MusicSlider.onValueChanged.AddListener(ChangeMusic);
        SFXSlider.onValueChanged.AddListener(ChangeSfx);
        DialogSlider.onValueChanged.AddListener(ChangeDialog);
        AmbienceSlider.onValueChanged.AddListener(ChangeAmbience);

        FancyFontButton.onClick.AddListener(SetFontFancy);
        NormalFontButton.onClick.AddListener(SetFontNormal);
        DyslexiaFontButton.onClick.AddListener(SetFontDyslexia);

        ClickDragOn.onClick.AddListener(EnableClickNDrag);
        ClickDragOff.onClick.AddListener(DisableClickNDrag);
    }

    // ---------------- Visual ----------------

    public void SetFullscreen() => SetWindowed(false);
    public void SetWindowed() => SetWindowed(true);

    public void SetWindowed(bool windowed)
    {
        Settings.Instance.isWindowed = windowed;
        FullscreenButton.interactable = windowed;
        WindowedButton.interactable = !windowed;
        if (SaveOnChange) SaveVisuals();
    }

    public void ChangeContrast(float value)
    {
        Settings.Instance.contrast = (int)value;
        ContrastLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveVisuals();
    }

    public void ChangeBrightness(float value)
    {
        Settings.Instance.brightness = (int)value;
        BrightnessLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveVisuals();
    }

    // ---------------- Audio ----------------

    public void ChangeMusic(float value)
    {
        Settings.Instance.music = (int)value;
        MusicLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveAudio();
    }

    public void ChangeSfx(float value)
    {
        Settings.Instance.SFX = (int)value;
        SFXLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveAudio();
    }

    public void ChangeDialog(float value)
    {
        Settings.Instance.dialog = (int)value;
        DialogLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveAudio();
    }

    public void ChangeAmbience(float value)
    {
        Settings.Instance.ambience = (int)value;
        AmbienceLabel.text = ((int)value).ToString();
        if (SaveOnChange) SaveAudio();
    }

    // ---------------- Font ----------------

    public void SetFontFancy() => SetFontStyle(0);
    public void SetFontNormal() => SetFontStyle(0);
    public void SetFontDyslexia() => SetFontStyle(0);

    public void SetFontStyle(int fontStyle)
    {
        Settings.Instance.textFont = fontStyle;
        if (SaveOnChange) SaveControls();
    }

    // ----------- Camera Movement -----------

    public void EnableClickNDrag() => SetClickAndDrag(true);
    public void DisableClickNDrag() => SetClickAndDrag(false);

    public void SetClickAndDrag(bool useClickNDrag)
    {
        Settings.Instance.useClickNDrag = useClickNDrag;
        if (SaveOnChange) SaveControls();
    }


    // ----------- Save Functions -----------

    public void SaveVisuals()
    {
        Settings.Instance.SaveVisualSettings();
    }

    public void SaveAudio()
    {
        Settings.Instance.SaveAudioSettings();
    }

    public void SaveControls()
    {
        Settings.Instance.SaveControlSettings();
    }
}
