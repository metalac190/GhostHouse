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

        SerializedProperty continueKeyCodeProperty;

        SerializedProperty characterDataProperty;
        SerializedProperty characterNameInLineProperty;
        SerializedProperty leftViewProperty;
        SerializedProperty rightViewProperty;

        public void OnEnable()
        {
            useFadeEffectProperty = serializedObject.FindProperty("_useFadeEffect");
            fadeInTimeProperty = serializedObject.FindProperty("_fadeInTime");
            fadeOutTimeProperty = serializedObject.FindProperty("_fadeOutTime");
            inBufferTimeProperty = serializedObject.FindProperty("_inBufferTime");

            useTypewriterEffectProperty = serializedObject.FindProperty("_useTypewriterEffect");
            typewriterEffectSpeedProperty = serializedObject.FindProperty("_typewriterEffectSpeed");

            continueKeyCodeProperty = serializedObject.FindProperty("_continueKeyCode");

            characterDataProperty = serializedObject.FindProperty("_characterData");
            characterNameInLineProperty = serializedObject.FindProperty("_characterNameInLine");
            leftViewProperty = serializedObject.FindProperty("_leftView");
            rightViewProperty = serializedObject.FindProperty("_rightView");
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

            // continue mode
            EditorGUILayout.PropertyField(continueKeyCodeProperty);

            // UI references
            EditorGUILayout.PropertyField(characterDataProperty);
            EditorGUILayout.PropertyField(characterNameInLineProperty);
            EditorGUILayout.PropertyField(leftViewProperty);
            EditorGUILayout.PropertyField(rightViewProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif