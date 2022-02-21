using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image))]
public class FadeToBlackAnimation : MonoBehaviour
{
    #region private variables
    [Header("UI References")]
    [SerializeField] bool _addYarnCommand = false;
    [SerializeField] Yarn.Unity.DialogueRunner _dialogueRunner = null;

    [Header("Animation Configurations")]
    [Tooltip("Time for linear fade in.")]
    [SerializeField] float _fadeIn = 1f;

    [Tooltip("Time for linear fade out.")]
    [SerializeField] float _fadeOut = 1f;

    [Tooltip("Initial color of fade in.")]
    [SerializeField] Color _initColor = Color.clear;

    [Tooltip("Final color of fade in and initial color of fade out.")]
    [SerializeField] Color _peakColor = Color.black;

    [Tooltip("Final color of fade out.")]
    [SerializeField] Color _finlColor = Color.clear;

    Image _img;
    #endregion

    #region Monobehaviour
    void Awake()
    {
        if (_addYarnCommand)
        {
            AddYarnCommands();
        }

        _img = GetComponent<Image>();
        _img.enabled = false;
    }

    void OnDisable()
    {
        _img.enabled = false;
        StopAllCoroutines();
    }
    #endregion

    /// <summary>
    /// Adds YarnCommands to <see cref="_dialogueRunner"/>
    /// </summary>
    void AddYarnCommands()
    {
        if (_dialogueRunner == null)
        {
            Debug.LogWarning("No Dialogue Runner was provided. Please provide one or turn _addYarnCommand off.");
        }
        else
        {
            // fade in
            _dialogueRunner.AddCommandHandler("fade_in", () =>
            {
                return FadeIn();
            });
            _dialogueRunner.AddCommandHandler("fade_in_unlocked", () =>
            {
                FadeIn();
            });

            // fade out
            _dialogueRunner.AddCommandHandler("fade_out", () =>
            {
                return FadeOut();
            });
            _dialogueRunner.AddCommandHandler("fade_out_unlocked", () =>
            {
                FadeOut();
            });

            // fade in and out
            _dialogueRunner.AddCommandHandler("fade_in_out", () =>
            {
                return FadeInOut();
            });
            _dialogueRunner.AddCommandHandler("fade_in_out_unlocked", () =>
            {
                FadeInOut();
            });
        }
    }

    #region animations
    /// <summary>
    /// Begins the fade in animation of <see cref="_img"/>
    /// </summary>
    /// <returns></returns>
    public Coroutine FadeIn()
    {
        return StartCoroutine(LerpColor(_img, _initColor, _peakColor, _fadeIn));
    }

    /// <summary>
    /// Begins the fade out animation of <see cref="_img"/>
    /// </summary>
    /// <returns></returns>
    public Coroutine FadeOut()
    {
        return StartCoroutine(LerpColor(_img, _peakColor, _finlColor, _fadeOut));
    }

    /// <summary>
    /// Begins the <see cref="FadeIn"/> animation of <see cref="_img"/> followed by <see cref="FadeOut"/>
    /// </summary>
    /// <returns></returns>
    public Coroutine FadeInOut()
    {
        return StartCoroutine(FadeInOutCouroutine());
    }

    /// <summary>
    /// Wrapper coroutine of <see cref="FadeIn"/> and <see cref="FadeOut"/>
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInOutCouroutine()
    {
        yield return FadeIn();
        yield return FadeOut();
    }
    #endregion

    /// <summary>
    /// Lerps <paramref name="graphic"/>.color from <paramref name="initColor"/> to <paramref name="finalColor"/>
    /// over <paramref name="duration"/> seconds. If provided, <paramref name="OnComplete"/> will be called when done.
    /// </summary>
    /// <param name="graphic"></param>
    /// <param name="initColor"></param>
    /// <param name="finalColor"></param>
    /// <param name="duration"></param>
    /// <param name="OnComplete"></param>
    /// <returns></returns>
    static IEnumerator LerpColor(MaskableGraphic graphic, Color initColor, Color finalColor, float duration, System.Action OnComplete = null)
    {
        // set initial color
        graphic.enabled = true;
        graphic.color = initColor;

        // color animation
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            graphic.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // set final color
        graphic.color = finalColor;

        if (OnComplete != null)
        {
            OnComplete();
        }

        yield break;
    }

    #region editor
#if UNITY_EDITOR
    [CustomEditor(typeof(FadeToBlackAnimation))]
    class FadeToBlackEditor : Editor
    {
        SerializedProperty _addYarnCommandProperty;
        SerializedProperty _dialogueRunnerProperty;
        SerializedProperty _fadeInProperty;
        SerializedProperty _fadeOutProperty;
        SerializedProperty _initColorProperty;
        SerializedProperty _peakColorProperty;
        SerializedProperty _finlColorProperty;

        public void OnEnable()
        {
            _addYarnCommandProperty = serializedObject.FindProperty(nameof(_addYarnCommand));
            _dialogueRunnerProperty = serializedObject.FindProperty(nameof(_dialogueRunner));
            _fadeInProperty = serializedObject.FindProperty(nameof(_fadeIn));
            _fadeOutProperty = serializedObject.FindProperty(nameof(_fadeOut));
            _initColorProperty = serializedObject.FindProperty(nameof(_initColor));
            _peakColorProperty = serializedObject.FindProperty(nameof(_peakColor));
            _finlColorProperty = serializedObject.FindProperty(nameof(_finlColor));
        }

        public override void OnInspectorGUI()
        {
            // camera
            EditorGUILayout.PropertyField(_addYarnCommandProperty);
            if (_addYarnCommandProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(_dialogueRunnerProperty);
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(_fadeInProperty);
            EditorGUILayout.PropertyField(_fadeOutProperty);
            EditorGUILayout.PropertyField(_initColorProperty);
            EditorGUILayout.PropertyField(_peakColorProperty);
            EditorGUILayout.PropertyField(_finlColorProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
