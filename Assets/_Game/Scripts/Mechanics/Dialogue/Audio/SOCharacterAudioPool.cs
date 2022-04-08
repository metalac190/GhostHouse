using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Dialog
{
    [CreateAssetMenu(menuName = "Sound System/Character Pool")]
    public class SOCharacterAudioPool : ScriptableObject
    {
        public List<SOCharacterAudio> Characters = new List<SOCharacterAudio>();
        public SOCharacterAudio Default = null;

        /// <summary>
        /// Searches <see cref="Characters"/> for the requested character.
        /// </summary>
        /// <param name="characterName"> The requested <see cref="SOCharacterAudio"/></param>
        /// <returns> <see cref="Default"/> if not found. </returns>
        public SOCharacterAudio GetCharacter(string characterName)
        {
            foreach (SOCharacterAudio charAudio in Characters)
            {
                if (charAudio != null && charAudio.CharacterName.ToLower() == characterName.ToLower())
                {
                    return charAudio;
                }
            }

            return Default;
        }
    }
}