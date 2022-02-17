using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound System/Character Audio Pool")]
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
            if (charAudio.CharacterName == characterName)
            {
                return charAudio;
            }
        }

        return Default;
    }
}