using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Clips.Base;

namespace Mechanics.Dialog
{
    [CreateAssetMenu(menuName = "Sound System/Character")]
    public class SOCharacterAudio : ScriptableObject
    {
        [Header("Character Data")]
        [Tooltip("non-case-sensitive name of character. This needs to match to what the narrative script uses for each character.")]
        public string CharacterName = null;

        [Tooltip("Any notes about this object you would like to leave. This is in now way used by the game.")]
        [TextArea] public string Description = null;

        [Header("Audio Clips")]
        public SfxBase PrimaryClip = null;
        public SfxBase SecondaryClip = null;
        public SfxBase TertiaryClip = null;
        public SfxBase QuaternaryClip = null;
    }
}