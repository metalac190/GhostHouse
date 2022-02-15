#if UNITY_EDITOR
using System.Text.RegularExpressions;
using Boo.Lang;
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
            var sections = ConvertToMarkdown(text);
            foreach (var section in sections) {
                GUIStyle style = new GUIStyle {
                    wordWrap = true,
                    fontStyle = section.bold > 0 ? FontStyle.Bold : FontStyle.Normal
                };
                switch (section.bold) {
                    case 3:
                        style.fontSize = 14;
                        break;
                    case 2:
                        style.fontSize = 24;
                        break;
                    case 1:
                        style.fontSize = 32;
                        break;
                }
                if (!string.IsNullOrEmpty(text)) {
                    GUILayout.Label(section.text, style);
                }
            }
            GUILayout.Space(10);
        }

        private static List<MarkdownLine> ConvertToMarkdown(string text) {
            var lines = Regex.Split(text, "\n|\r|\r\n");

            var markdownLines = new List<MarkdownLine>();
            foreach (var l in lines) {
                var line = l.Trim();
                int bold = 0;
                if (line.StartsWith("###")) {
                    line = line.Remove(0, 3);
                    bold = 3;
                }
                if (line.StartsWith("##")) {
                    line = line.Remove(0, 2);
                    bold = 2;
                }
                if (line.StartsWith("#")) {
                    line = line.Remove(0, 1);
                    bold = 1;
                }
                var markdownLine = new MarkdownLine {
                    text = line.Trim(),
                    bold = bold
                };
                markdownLines.Add(markdownLine);
            }
            return markdownLines;
        }

        private struct MarkdownLine
        {
            public string text;
            public int bold;
        }
    }
}
#endif