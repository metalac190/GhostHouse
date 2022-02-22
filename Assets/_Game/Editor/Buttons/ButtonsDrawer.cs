#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using Utility.Buttons;

namespace _Game.Editor.Buttons
{
    public class ButtonsDrawer
    {
        public readonly List<Button> Buttons = new List<Button>();

        // Gather all methods and find ones with a Button Attribute
        public ButtonsDrawer(object target) {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);

            foreach (var method in methods) {
                ButtonAttribute buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute != null) {
                    // Create a button from the button attribute
                    Buttons.Add(Button.Create(method, buttonAttribute));
                }
            }
        }

        // Draw the buttons
        public void DrawButtons(IEnumerable<object> targets) {
            foreach (Button button in Buttons) {
                button.Draw(targets);
            }
        }
    }
}
#endif