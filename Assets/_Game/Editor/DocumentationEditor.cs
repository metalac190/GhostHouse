#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Utility;

namespace Assets._Game.Editor
{
    [CustomEditor(typeof(Documentation))]
    public class DocumentationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            var textProp = serializedObject.FindProperty("_documentationText");
            if (textProp != null) {
                var rect = EditorGUILayout.GetControlRect();
                EditorGUI.PropertyField(rect, textProp);
                serializedObject.ApplyModifiedProperties();
            }
            else {
                base.OnInspectorGUI();
            }
            GUILayout.Space(10);
            var text = ((Documentation)target).Text;
            GUIStyle style = new GUIStyle {
                wordWrap = true
            };
            if (!string.IsNullOrEmpty(text)) {
                GUILayout.Label(text, style);
            }
        }
    }
}
#endif