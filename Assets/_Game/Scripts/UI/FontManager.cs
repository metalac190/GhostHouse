using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FontManager : MonoBehaviour
{
    public static FontManager _instance = null;

    [Header("Font Assets")]
    [SerializeField]
    TMP_FontAsset _fancyFont = null;

    [SerializeField]
    TMP_FontAsset _normalFont = null;

    [SerializeField]
    TMP_FontAsset _dyslexiaFont = null;

    [SerializeField]
    bool _useNormalFont = true;

    void Awake()
    {
        // enforce singleton pattern
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        SceneManager.activeSceneChanged += UpdateAllTextInScene;
    }

    void OnDestroy()
    {
        if (_instance == null) return;
        SceneManager.activeSceneChanged -= UpdateAllTextInScene;
    }

    void Start() => UpdateAllText();

    /// <summary>
    /// Gets the font asset corresponding to the player's choice in <see cref="DataManager"/>
    /// </summary>
    /// <returns></returns>
    public TMP_FontAsset GetFont()
    {
        switch (DataManager.Instance.settingsTextFont)
        {
            default:
            case 0:
                return _fancyFont;

            case 1:
                return _normalFont;

            case 2:
                return _dyslexiaFont;
        }
    }

    void UpdateAllTextInScene(Scene previousScene, Scene nextScene)
    {
        // if this is the first scene loaded, let Start trigger UpdateAllText.
        if (previousScene == null) return;
        UpdateAllText();
    }

    /// <summary>
    /// Updates the fonts of text in scene to user preferences.
    /// </summary>
    void UpdateAllText()
    {
        // get all text mesh pro components in the scene
        TMP_Text[] textInCurScene = Resources.FindObjectsOfTypeAll<TMP_Text>();
        for (int i = 0; i < textInCurScene.Length; i++)
        {
            if (string.IsNullOrEmpty(textInCurScene[i].gameObject.scene.name))
                textInCurScene[i] = null;
        }

        TMP_FontAsset fontAsset = _useNormalFont? _normalFont : GetFont();

        foreach (var text in textInCurScene)
        {
            if (text == null) continue;
            try
            {
                text.font = fontAsset;
            }
            catch (System.ArgumentException)
            {
                Debug.Log("Here is mystery exception! Not sure why it bug, but it do and it don't matter :).");
            }
        }
    }
}

public enum FontMode
{
    Fancy, Normal, Dyslexia
}