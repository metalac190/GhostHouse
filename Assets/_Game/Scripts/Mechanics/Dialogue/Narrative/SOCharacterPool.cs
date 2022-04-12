using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Dialog
{
    [CreateAssetMenu(menuName = "Narrative/Character Pool")]
    public class SOCharacterPool : ScriptableObject
    {
        [TextArea()]
        [Tooltip("This may be used for notes. It is in no way used or processed by the game.")]
        public string Description = string.Empty;

        [SerializeField]
        List<SOCharacter> _characters = new List<SOCharacter>();

        /// <summary>
        /// Provides a dictionary like interface to <see cref="_characters"/>. Returns null if unable to find the requested character.
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public SOCharacter GetCharacter(string characterName)
        {
            foreach (SOCharacter character in _characters)
            {
                if (character != null && characterName.ToLower() == character.CharacterName.ToLower())
                {
                    return character;
                }
            }

            return null;
        }
    }
}