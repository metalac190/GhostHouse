using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialog
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

        /// <summary>
        /// Finds the corresponding sprite for <paramref name="emotion"/> from <see cref="_sprites"/>.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns> null if unable to find it.</returns>
        public Sprite GetSprite(CharacterEmotion emotion)
        {
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
            switch(str.ToLower())
            {
                case "surprise":
                case "surprised":
                    return CharacterEmotion.Surprise;

                default:
                    Debug.LogWarning($"Unable to find CharacterEmotion for {str}");
                    return CharacterEmotion.Neutral;

                case "neutral":
                    return CharacterEmotion.Neutral;
            }
        }
    }

    public enum CharacterEmotion
    {
        Neutral = 0, Surprise = 1
    }
}