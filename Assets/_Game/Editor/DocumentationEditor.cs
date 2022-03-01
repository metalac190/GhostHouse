#if UNITY_EDITOR
using System.Text.RegularExpressions;
using Boo.Lang;
using UnityEditor;
using UnityEngine;
using Utility;

namespace _Game.Editor
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
            var text = ((Documentation)target).Text;
            var sections = ConvertToMarkdown(text);
            foreach (var section in sections) {
                GUIStyle style = new GUIStyle {
                    wordWrap = true
                };
                int space = 6;
                switch (section.Bold) {
                    case 1:
                        space = 12;
                        style.fontSize = 24;
                        style.fontStyle = FontStyle.Bold;
                        break;
                    case 2:
                        space = 20;
                        style.fontSize = 16;
                        style.fontStyle = FontStyle.Bold;
                        break;
                    case 3:
                        space = 20;
                        style.fontSize = 14;
                        style.fontStyle = FontStyle.Italic;
                        break;
                }
                if (!string.IsNullOrEmpty(section.Text)) {
                    GUILayout.Space(space);
                    GUILayout.Label(section.Text, style);
                }
            }
            GUILayout.Space(16);
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
                    Text = line.Trim(),
                    Bold = bold
                };
                markdownLines.Add(markdownLine);
            }
            return markdownLines;
        }

        private struct MarkdownLine
        {
            public string Text;
            public int Bold;
        }
    }
}
#endif