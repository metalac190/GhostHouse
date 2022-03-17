#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Game.Dialog;

namespace Game.Dialog
{
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
        SerializedProperty characterDataProperty;
        SerializedProperty characterNameInLineProperty;
        SerializedProperty continueButtonProperty;
        SerializedProperty uiParentProperty;
        SerializedProperty progressbarProperty;

        SerializedProperty continueActionTypeProperty;
        SerializedProperty continueActionKeyCodeProperty;

        public void OnEnable()
        {
            useFadeEffectProperty = serializedObject.FindProperty("_useFadeEffect");
            fadeInTimeProperty = serializedObject.FindProperty("_fadeInTime");
            fadeOutTimeProperty = serializedObject.FindProperty("_fadeOutTime");

            useTypewriterEffectProperty = serializedObject.FindProperty("_useTypewriterEffect");
            typewriterEffectSpeedProperty = serializedObject.FindProperty("_typewriterEffectSpeed");

            lineTextProperty = serializedObject.FindProperty("_lineText");
            characterPortraitImageProperty = serializedObject.FindProperty("_characterPortraitImage");
            characterDataProperty = serializedObject.FindProperty("_charactersData");
            characterNameTextProperty = serializedObject.FindProperty("_characterNameText");
            characterNameInLineProperty = serializedObject.FindProperty("_characterNameInLine");
            continueButtonProperty = serializedObject.FindProperty("_continueButton");
            uiParentProperty = serializedObject.FindProperty("_uiParent");
            progressbarProperty = serializedObject.FindProperty("_progressbar");

            continueActionTypeProperty = serializedObject.FindProperty("_continueActionType");
            continueActionKeyCodeProperty = serializedObject.FindProperty("_continueActionKeyCode");
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
            if (characterPortraitImageProperty.objectReferenceValue != null)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(characterDataProperty);
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(characterNameTextProperty);
            if (characterNameTextProperty.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(characterNameInLineProperty);
            }

            EditorGUILayout.PropertyField(continueButtonProperty);
            EditorGUILayout.PropertyField(uiParentProperty);
            EditorGUILayout.PropertyField(progressbarProperty);

            // continue mode
            EditorGUILayout.PropertyField(continueActionTypeProperty);
            if (continueActionTypeProperty.enumValueIndex != 0)
            {
                EditorGUILayout.PropertyField(continueActionKeyCodeProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif