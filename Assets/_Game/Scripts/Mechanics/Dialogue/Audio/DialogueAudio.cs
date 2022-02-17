using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Clips.Base;

public class DialogueAudio : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The CharacterView UI this should listen to.")]
    CharacterView _characterView = null;

    [SerializeField]
    SOCharacterAudioPool _characterAudioPool = null;

    SOCharacterAudio _speaker = null;

    void OnEnable()
    {
        if (_characterView == null)
        {
            Debug.LogWarning($"No CharacterView was provided. Disabling \"{name}\" DialogueAudio component.");
            this.enabled = false;
        }
        else
        {
            _characterView.OnCharacterTyped += PlayDialogueAudioClip;
        }

        if (_characterAudioPool == null)
        {
            Debug.LogError($"\"{name}\" is unable to play dialogue audio. No SOCharacterAudioPool was provided to _characterAudioPool.");
            this.enabled = false;
        }
    }

    void OnDisable()
    {
        if (_characterView != null)
        {
            _characterView.OnCharacterTyped -= PlayDialogueAudioClip;
        }
    }

    /// <summary>
    /// Finds the appropriate audio clip to play (if any) and sends it to the audio manager to play.
    /// </summary>
    /// <param name="line"> The line of text being played. </param>
    /// <param name="index"> The index of the last letter typed. </param>
    /// TODO: if text speed is fast enough, so multiple letters are played at a time,
    ///     they will be missed in the below string analysis
    void PlayDialogueAudioClip(Yarn.Unity.LocalizedLine line, int index)
    {
        // update the speaker if necessary
        if (_speaker == null || _speaker.CharacterName != line.CharacterName)
        {
            _speaker = _characterAudioPool.GetCharacter(line.CharacterName);
        }

        if (_speaker == null) { return; }

        // isolate token
        string token = line.TextWithoutCharacterName.Text[index].ToString();
        SfxBase sfx = _speaker.GetAudio(token);
        if (sfx != null)
        {
            Debug.Log(sfx.name);
        }
    }
}
