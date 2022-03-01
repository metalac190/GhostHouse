#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utility.Buttons;

namespace _Game.Editor.Buttons
{
    internal class ButtonWithoutParams : Button
    {
        public ButtonWithoutParams(MethodInfo method, ButtonAttribute buttonAttribute)
            : base(method, buttonAttribute) {
        }

        protected override void DrawInternal(IEnumerable<object> targets, int spacing) {
            if (spacing > 0) {
                GUILayout.Space(spacing);
            }
            if (GUILayout.Button(DisplayName)) {
                foreach (object obj in targets) {
                    Method.Invoke(obj, null);
                }
            }
        }
    }
}
#endif