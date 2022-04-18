using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Dialog
{
    [CreateAssetMenu(menuName = "Narrative/Character")]
    public class SOCharacter : ScriptableObject
    {
        public string CharacterName = string.Empty;

        [TextArea()]
        [Tooltip("This may be used a notes section. It is in now way used or processed by the game.")]
        public string Description = string.Empty;

        [SerializeField]
        List<Sprite> _sprites = null;

        [Header("Options")]
        public bool ShowPortrait = true;
        public bool ShowName = true;
        public bool PlayAudio = true;

        [Header("Alternate Options")]
        [Tooltip("If unchecked, the default dialog box sprite and color will be used and the below variables will be ignored.")]
        public bool UseAlternateBoxStyle = false;
        public Sprite AlternateBoxSprite = null;
        public Color AlternateBoxColor = Color.white;

        /// <summary>
        /// Finds the corresponding sprite for <paramref name="emotion"/> from <see cref="_sprites"/>.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns> null if unable to find it.</returns>
        public Sprite GetSprite(CharacterEmotion emotion) {
            if (_sprites.Count == 0)
            {
                Debug.LogWarning($"character file \"{name}\". {CharacterName} has no sprites.");
                return null;
            }
            else if ((int) emotion >= _sprites.Count)
            {
                Debug.LogWarning($"character file \"{name}\". {CharacterName} does not have the sprite for {emotion}.");
                return null;
            }

            return _sprites[(int) emotion];
        }

        /// <summary>
        /// Converts a <paramref name="str"/> to <see cref="CharacterEmotion"/> by comparing to <see cref="CharacterEmotion"/>'s possible values
        /// </summary>
        /// <param name="str"></param>
        /// <returns> <see cref="CharacterEmotion.Neutral"/> if unable to find requested value. </returns>
        public static CharacterEmotion StringToEmotion(string str)
        {
            switch (str.ToLower())
            {
                case "angry":
                case "anger":
                    return CharacterEmotion.Angry;

                case "happy":
                    return CharacterEmotion.Happy;

                case "neutral":
                case "idle":
                    return CharacterEmotion.Idle;

                case "surprise":
                case "surprised":
                    return CharacterEmotion.Surprised;

                case "sad":
                    return CharacterEmotion.Sad;

                default:
                    Debug.LogWarning($"Unable to find CharacterEmotion for {str}");
                    return CharacterEmotion.Sad;
            }
        }
    }

    public enum CharacterEmotion
    {
        Angry = 0, Happy = 1, Idle = 2, Sad = 3, Surprised = 4
    }
}