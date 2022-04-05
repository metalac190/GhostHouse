using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Communication between the Settings UI and Settings singleton
public class PanelMediator : MonoBehaviour
{
    //The UI needs to be up to date with everything
    private void Update()
    {
    }

    private void OnEnable() {
        DragSpeedSlider.value = Settings.Instance.dragSpeed;
        MusicSlider.value = Settings.Instance.music;
        SFXSlider.value = Settings.Instance.SFX;
        DialogSlider.value = Settings.Instance.dialog;
        AmbienceSlider.value = Settings.Instance.ambience;
        ContrastSlider.value = Settings.Instance.contrast;
        BrightnessSlider.value = Settings.Instance.brightness;
    }

    //Methods to save UI values
    public void SaveControls()
    {
        Settings.Instance.SaveControlSettings();
    }

    public void SaveAudio()
    {
        Settings.Instance.SaveAudioSettings();
    }

    public void SaveVisuals()
    {
        Settings.Instance.SaveVisualSettings();
    }

    //References to UI values

    public Slider DragSpeedSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider DialogSlider;
    public Slider AmbienceSlider;
    public Slider ContrastSlider;
    public Slider BrightnessSlider;

    public TextMeshProUGUI DragSpeedLabel;
    public TextMeshProUGUI MusicLabel;
    public TextMeshProUGUI SFXLabel;
    public TextMeshProUGUI DialogLabel;
    public TextMeshProUGUI AmbienceLabel;
    public TextMeshProUGUI ContrastLabel;
    public TextMeshProUGUI BrightnessLabel;

    public void MouseInteractClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.leftClickInteract = true;
        else Settings.Instance.leftClickInteract = false;
    }

    public void WASDClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.useWASD = true;
        else Settings.Instance.useWASD = false;
    }

    public void ClickNDrag(int btnID)
    {
        if (btnID == 0) Settings.Instance.useClickNDrag = true;
        else Settings.Instance.useClickNDrag = false;
    }

    public void ChangeDragSpeed()
    {
        Settings.Instance.dragSpeed = (int)DragSpeedSlider.value;
        DragSpeedLabel.text = ((int)DragSpeedSlider.value).ToString();
    }

    public void ArrowsClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.useArrowKeys = true;
        else Settings.Instance.useArrowKeys = false;
    }

    public void ChangeMusic()
    {
        Settings.Instance.music = (int)MusicSlider.value;
        MusicLabel.text = ((int)MusicSlider.value).ToString();
    }

    public void ChangeSFX()
    {
        Settings.Instance.SFX = (int)SFXSlider.value;
        SFXLabel.text = ((int)SFXSlider.value).ToString();
    }

    public void ChangeDialog()
    {
        Settings.Instance.dialog = (int)DialogSlider.value;
        DialogLabel.text = ((int)DialogSlider.value).ToString();
    }

    public void ChangeAmbience()
    {
        Settings.Instance.ambience = (int)AmbienceSlider.value;
        AmbienceLabel.text = ((int)AmbienceSlider.value).ToString();
    }

    public void WindowedClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.isWindowed = true;
        else Settings.Instance.isWindowed = false;
    }

    public void ChangeContrast()
    {
        Settings.Instance.contrast = (int)ContrastSlider.value;
        ContrastLabel.text = ((int)ContrastSlider.value).ToString();
    }

    public void ChangeBrightness()
    {
        Settings.Instance.brightness = (int)BrightnessSlider.value;
        BrightnessLabel.text = ((int)BrightnessSlider.value).ToString();
    }

    public void GUIFontClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.largeGUIFont = true;
        else Settings.Instance.largeGUIFont = false;
    }

    public void TextFontClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.largeTextFont = true;
        else Settings.Instance.largeTextFont = false;
    }

    public void FontStyleClick(int btnID)
    {
        if (btnID == 0) Settings.Instance.textFont = 0;
        else if (btnID == 1) Settings.Instance.textFont = 1;
        else if (btnID == 2) Settings.Instance.textFont = 2;
    }
}
