using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Clips.Base;

[CreateAssetMenu(menuName = "Sound System/Character Audio")]
public class SOCharacterAudio : ScriptableObject
{
    public string CharacterName = null;
    [TextArea] public string Description = null;
    public List<SyllablePool> Syllables = new List<SyllablePool>();

    /// <summary>
    /// Finds the appropriate <see cref="SyllablePool"/> in <see cref="Syllables"/> and gets a random clip from it.
    /// </summary>
    /// <param name="token"></param>
    /// <returns> Will return null if unable to find appropriate audio clip. </returns>
    public SfxBase GetAudio(string token)
    {
        foreach (SyllablePool syllablePool in Syllables)
        {
            if (syllablePool.MatchesTrigger(token))
            {
                return syllablePool.GetRandomClip();
            }
        }

        return null;
    }
}

[System.Serializable]
public class SyllablePool
{
    public string Trigger = null;
    public List<SfxBase> Clips = new List<SfxBase>();

    /// <summary>
    /// Compares token to <see cref="Trigger"/>.
    /// </summary>
    /// <param name="token"></param>
    /// <returns> True if this syllable pool should be activated from. </returns>
    public bool MatchesTrigger(string token)
    {
        return Trigger == token;
    }

    /// <summary>
    /// Get a random <see cref="SfxBase"/> from <see cref="Clips"/> or null if empty.
    /// </summary>
    /// <returns></returns>
    public SfxBase GetRandomClip()
    {
        if (Clips.Count == 0) { return null; }
        return Clips[(int) Random.Range(0, Clips.Count)];
    }
}