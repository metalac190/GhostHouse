using UnityEngine;
using UnityEditor;

/// <summary>
/// A recycled LineViewEditor.cs from YarnSpinner 2.0.2 for CharcterView.cs
/// </summary>
[CustomEditor(typeof(CharacterView))]
public class CharacterViewEditor : Editor
{
    SerializedProperty useFadeEffectProperty;
    SerializedProperty fadeInTimeProperty;
    SerializedProperty fadeOutTimeProperty;

    SerializedProperty useTypewriterEffectProperty;
    SerializedProperty typewriterEffectSpeedProperty;

    SerializedProperty lineTextProperty;
    SerializedProperty characterNameTextProperty;
    SerializedProperty characterPortraitImageProperty;
    SerializedProperty characterNameInLineProperty;
    SerializedProperty continueButtonProperty;

    SerializedProperty continueActionTypeProperty;
    SerializedProperty continueActionKeyCodeProperty;

    public void OnEnable()
    {
        useFadeEffectProperty = serializedObject.FindProperty("useFadeEffect");
        fadeInTimeProperty = serializedObject.FindProperty("fadeInTime");
        fadeOutTimeProperty = serializedObject.FindProperty("fadeOutTime");

        useTypewriterEffectProperty = serializedObject.FindProperty("useTypewriterEffect");
        typewriterEffectSpeedProperty = serializedObject.FindProperty("typewriterEffectSpeed");

        lineTextProperty = serializedObject.FindProperty("lineText");
        characterPortraitImageProperty = serializedObject.FindProperty("characterPortraitImage");
        characterNameTextProperty = serializedObject.FindProperty("characterNameText");
        characterNameInLineProperty = serializedObject.FindProperty("characterNameInLine");
        continueButtonProperty = serializedObject.FindProperty("continueButton");

        continueActionTypeProperty = serializedObject.FindProperty("continueActionType");
        continueActionKeyCodeProperty = serializedObject.FindProperty("continueActionKeyCode");
    }

    public override void OnInspectorGUI()
    {
        // fade effect
        EditorGUILayout.PropertyField(useFadeEffectProperty);
        if (useFadeEffectProperty.boolValue)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(fadeInTimeProperty);
            EditorGUILayout.PropertyField(fadeOutTimeProperty);
            EditorGUI.indentLevel -= 1;
        }

        // typewriter effect
        EditorGUILayout.PropertyField(useTypewriterEffectProperty);
        if (useTypewriterEffectProperty.boolValue)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(typewriterEffectSpeedProperty);
            EditorGUI.indentLevel -= 1;
        }

        // UI references
        EditorGUILayout.PropertyField(lineTextProperty);
        EditorGUILayout.PropertyField(characterPortraitImageProperty);

        EditorGUILayout.PropertyField(characterNameTextProperty);
        if (characterNameTextProperty.objectReferenceValue == null)
        {
            EditorGUILayout.PropertyField(characterNameInLineProperty);
        }

        EditorGUILayout.PropertyField(continueButtonProperty);

        // continue mode
        EditorGUILayout.PropertyField(continueActionTypeProperty);
        if (continueActionTypeProperty.enumValueIndex != 0)
        {
            EditorGUILayout.PropertyField(continueActionKeyCodeProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}