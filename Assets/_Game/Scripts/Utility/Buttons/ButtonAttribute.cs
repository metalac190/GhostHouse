using System;

namespace Utility.Buttons
{
    public enum ButtonMode
    {
        Always,
        NotPlaying,
        WhilePlaying
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ButtonAttribute : Attribute
    {
        public readonly string name;

        public ButtonAttribute()
        {
        }

        public ButtonAttribute(string name) => this.name = name;

        public ButtonMode Mode { get; set; } = ButtonMode.Always;

        public int Spacing { get; set; } = 0;
    }
}