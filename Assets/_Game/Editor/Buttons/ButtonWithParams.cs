#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _Game.Editor.Buttons.Utils;
using UnityEditor;
using UnityEngine;
using Utility.Buttons;

namespace _Game.Editor.Buttons
{
    using Object = UnityEngine.Object;

    internal class ButtonWithParams : Button
    {
        private const float ButtonWidth = 100f;
        private const string ButtonTitle = "Invoke";

        private readonly Parameter[] _parameters;
        private bool _expanded;

        public ButtonWithParams(MethodInfo method, ButtonAttribute buttonAttribute, IEnumerable<ParameterInfo> parameters)
            : base(method, buttonAttribute) {
            _parameters = parameters.Select(paramInfo => new Parameter(paramInfo)).ToArray();
            _expanded = true;
        }

        // TODO: Draw default values for parameters

        protected override void DrawInternal(IEnumerable<object> targets, int spacing) {
            if (spacing > 0) {
                GUILayout.Space(spacing);
            }
            Rect foldoutRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.foldoutHeader);
            _expanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, _expanded, DisplayName);
            if (_expanded) {
                EditorGUI.indentLevel++;
                foreach (Parameter param in _parameters) {
                    param.Draw();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (_expanded) {
                if (GUILayout.Button(DisplayName)) {
                    foreach (object obj in targets) {
                        Method.Invoke(obj, _parameters.Select(param => param.Value).ToArray());
                    }
                }
            }
        }

        private readonly struct Parameter
        {
            private readonly FieldInfo _fieldInfo;
            private readonly ScriptableObject _scriptableObj;
            private readonly NoScriptFieldEditor _editor;

            public Parameter(ParameterInfo paramInfo) {
                Type generatedType = ScriptableObjectCache.GetClass(paramInfo.Name, paramInfo.ParameterType);
                _scriptableObj = ScriptableObject.CreateInstance(generatedType);
                _fieldInfo = generatedType.GetField(paramInfo.Name);
                _editor = CreateEditor<NoScriptFieldEditor>(_scriptableObj);
            }

            public object Value {
                get {
                    _editor.ApplyModifiedProperties();
                    return _fieldInfo.GetValue(_scriptableObj);
                }
            }

            public void Draw() {
                _editor.OnInspectorGUI();
            }

            private static TEditor CreateEditor<TEditor>(Object obj)
                where TEditor : UnityEditor.Editor {
                return (TEditor)UnityEditor.Editor.CreateEditor(obj, typeof(TEditor));
            }
        }
    }
}
#endif