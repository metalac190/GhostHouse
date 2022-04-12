using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class GraphicsController : MonoBehaviour
{
    public static GraphicsController Instance { get; private set; } = null;

    static FullScreenMode _screenMode = FullScreenMode.FullScreenWindow;
    public static FullScreenMode ScreenMode
    {
        get { return _screenMode; }
        set
        {
            _screenMode = value;
            UpdateScreenMode();
        }
    }

    static float _exposure;
    public static float Exposure
    {
        get { return _exposure; }
        set
        {
            _exposure = value;
            Instance?.SetExposure();
        }
    }

    static float _contrast;
    public static float Contrast
    {
        get { return _contrast; }
        set
        {
            _contrast = value;
            Instance?.SetContrast();
        }
    }

    [SerializeField]
    Vector2 _exposureBounds = new Vector2(-100f, 100f);

    [SerializeField]
    Vector2 _contrastBounds = new Vector2(-100, 100f);

    ColorAdjustments _colorAdjustments = null;
    float _initExposure, _initContrast;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        VolumeProfile profile = GetComponent<Volume>()?.sharedProfile;
        if (profile == null) return;

        // ensure we have a ColorAdjustments component
        if (!profile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments = profile.Add<ColorAdjustments>();
        }

        // enable necessary settings
        _colorAdjustments.postExposure.overrideState = true;
        _colorAdjustments.contrast.overrideState = true;

        _initExposure = _colorAdjustments.postExposure.value;
        _initContrast = _colorAdjustments.contrast.value;

        SetExposure();
        SetContrast();
    }

    void OnDestroy()
    {
        if (Instance != this) return;

        Instance = null;

        if (_colorAdjustments != null) {
            _colorAdjustments.postExposure.value = _initExposure;
            _colorAdjustments.contrast.value = _initContrast;
        }
    }

    public static void UpdateScreenMode()
    {
        Screen.fullScreenMode = _screenMode;
    }

    void SetContrast()
    {
        if (_colorAdjustments == null) return;
        _colorAdjustments.contrast.value = Mathf.Clamp(_contrast, _contrastBounds.x, _contrastBounds.y);
    }

    void SetExposure()
    {
        if (_colorAdjustments == null) return;
        _colorAdjustments.postExposure.value = Mathf.Clamp(_exposure, _exposureBounds.x, _exposureBounds.y);
    }
}