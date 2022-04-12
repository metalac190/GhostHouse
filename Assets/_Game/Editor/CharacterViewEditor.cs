#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Mechanics.Dialog;

namespace _Game.Editor
{
    /// <summary>
    /// A recycled LineViewEditor.cs from YarnSpinner 2.0.2 for CharcterView.cs
    /// </summary>
    [CustomEditor(typeof(CharacterView))]
    public class CharacterViewEditor : UnityEditor.Editor
    {
        SerializedProperty useFadeEffectProperty;
        SerializedProperty fadeInTimeProperty;
        SerializedProperty fadeOutTimeProperty;
        SerializedProperty inBufferTimeProperty;

        SerializedProperty useTypewriterEffectProperty;
        SerializedProperty typewriterEffectSpeedProperty;

        SerializedProperty dialogImageProperty;
        SerializedProperty alternateCharactersProperty;
        SerializedProperty alternateDialogSpriteProperty;
        SerializedProperty alternateDialogColorProperty;

        SerializedProperty continueKeyCodeProperty;

        SerializedProperty lineTextProperty;
        SerializedProperty characterNameObjectProperty;
        SerializedProperty characterNameTextProperty;
        SerializedProperty characterPortraitImageProperty;
        SerializedProperty characterDataProperty;
        SerializedProperty characterNameInLineProperty;
        SerializedProperty continueButtonProperty;
        SerializedProperty uiParentProperty;
        SerializedProperty progressbarProperty;

        public void OnEnable()
        {
            useFadeEffectProperty = serializedObject.FindProperty("_useFadeEffect");
            fadeInTimeProperty = serializedObject.FindProperty("_fadeInTime");
            fadeOutTimeProperty = serializedObject.FindProperty("_fadeOutTime");
            inBufferTimeProperty = serializedObject.FindProperty("_inBufferTime");

            useTypewriterEffectProperty = serializedObject.FindProperty("_useTypewriterEffect");
            typewriterEffectSpeedProperty = serializedObject.FindProperty("_typewriterEffectSpeed");

            dialogImageProperty = serializedObject.FindProperty("_dialogImage");
            alternateCharactersProperty = serializedObject.FindProperty("_alternateCharacters");
            alternateDialogSpriteProperty = serializedObject.FindProperty("_alternateDialogSprite");
            alternateDialogColorProperty = serializedObject.FindProperty("_alternateDialogColor");

            continueKeyCodeProperty = serializedObject.FindProperty("_continueKeyCode");

            lineTextProperty = serializedObject.FindProperty("_lineText");
            characterPortraitImageProperty = serializedObject.FindProperty("_characterPortraitImage");
            characterDataProperty = serializedObject.FindProperty("_charactersData");
            characterNameObjectProperty = serializedObject.FindProperty("_characterNameObject");
            characterNameTextProperty = serializedObject.FindProperty("_characterNameText");
            characterNameInLineProperty = serializedObject.FindProperty("_characterNameInLine");
            continueButtonProperty = serializedObject.FindProperty("_continueButton");
            uiParentProperty = serializedObject.FindProperty("_uiParent");
            progressbarProperty = serializedObject.FindProperty("_progressbar");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(inBufferTimeProperty);

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

            // alternate styles
            EditorGUILayout.PropertyField(alternateCharactersProperty);
            EditorGUILayout.PropertyField(dialogImageProperty);

            if (dialogImageProperty.objectReferenceValue != null)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(alternateDialogSpriteProperty);
                EditorGUILayout.PropertyField(alternateDialogColorProperty);
                EditorGUI.indentLevel -= 1;
            }

            // continue mode
            EditorGUILayout.PropertyField(continueKeyCodeProperty);

            // UI references
            EditorGUILayout.PropertyField(lineTextProperty);
            EditorGUILayout.PropertyField(characterNameObjectProperty);
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

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif