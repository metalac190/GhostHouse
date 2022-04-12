using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Clips.Base;
using Utility.Audio.Controllers;

namespace Mechanics.Dialog
{
    [RequireComponent(typeof(AudioSourceController))]
    public class DialogueAudio : MonoBehaviour
    {
        #region private variables
        [SerializeField]
        [Tooltip("After playing the primary audio clip, this is the chance of the secondary clip playing and 1-this is the chance of the tertiary clip playing.")]
        float _secondaryChance = .6f;

        [SerializeField]
        SOCharacterAudioPool _characterAudioPool = null;

        [SerializeField]
        [Tooltip("The CharacterView UI this match up to.")]
        CharacterView _characterView = null;

        AudioSourceController _audioSource = null;
        Yarn.Unity.LocalizedLine _line = null;
        SOCharacterAudio _speaker = null;
        bool _playLastClip = false;
        int _indexOfLastWord = -1;
        #endregion

        #region Monobehaviour
        void Awake()
        {
            _audioSource = GetComponent<AudioSourceController>();
        }

        void OnEnable()
        {
            if (_characterView == null)
            {
                Debug.LogWarning($"No CharacterView was provided. Disabling \"{name}\" DialogueAudio component.");
                this.enabled = false;
            }
            else
            {
                _characterView.OnLineStarted += OnLineStart;
                _characterView.OnCharacterTyped += OnLineUpdate;
                _characterView.OnLineEnd += OnLineEnd;
            }

            if (_characterAudioPool == null)
            {
                Debug.LogWarning($"\"{name}\" is unable to play dialogue audio. No SOCharacterAudioPool was provided to _characterAudioPool.");
                this.enabled = false;
            }
        }

        void OnDisable()
        {
            if (_characterView != null)
            {
                _characterView.OnLineStarted -= OnLineStart;
                _characterView.OnCharacterTyped -= OnLineUpdate;
                _characterView.OnLineEnd -= OnLineEnd;
            }
        }
        #endregion

        /// <summary>
        /// Begin audio loop.
        /// </summary>
        /// <param name="line"> Line of dialogue being played. </param>
        void OnLineStart(Yarn.Unity.LocalizedLine line)
        {
            _line = line;
            _speaker = _characterAudioPool.GetCharacter(line.CharacterName);
            _playLastClip = false;

            if (_speaker.PrimaryClip == null && _speaker.SecondaryClip == null
                && _speaker.TertiaryClip == null && _speaker.QuaternaryClip == null)
            {
                Debug.LogWarning($"\"{line.CharacterName}\" has no dialog audio.");
                return;
            }

            _indexOfLastWord = line.TextWithoutCharacterName.Text.TrimEnd().LastIndexOf(" ");
            if (_indexOfLastWord < 0)
            {
                Debug.LogWarning("Unable to find the starting index of the last word. This line's dialog audio will not end with the quaternary clip.");
                _indexOfLastWord = line.TextWithoutCharacterName.Text.Length;
            }

            if (_speaker == null)
            {
                Debug.LogWarning($"Unable to find character \"{line.CharacterName}\". Dialog audio will not be played for this line.");
                _line = null;
            }

            StartCoroutine(PlayAudioLoop());
        }

        /// <summary>
        /// Triggered each time the dialogue display is updated. Tracks when audio loop should transition to final state.
        /// </summary>
        /// <param name="index"></param>
        void OnLineUpdate(int index)
        {
            if (index > _indexOfLastWord && !_playLastClip)
            {
                _playLastClip = true;
            }
        }

        /// <summary>
        /// Cleans up currently running coroutines.
        /// </summary>
        void OnLineEnd()
        {
            StopAllCoroutines();
        }

        IEnumerator PlayAudioLoop()
        {
            while (!_playLastClip)
            {
                yield return StartCoroutine(PlayPrimaryClip());
                yield return StartCoroutine(PlaySecondaryClip());
            }

            PlayClip(_speaker.QuaternaryClip);
            yield break;

            IEnumerator PlayPrimaryClip()
            {
                if (_speaker.PrimaryClip != null)
                {
                    float waitTime = PlayClip(_speaker.PrimaryClip);
                    yield return new WaitForSeconds(waitTime);
                }
                else
                {
                    Debug.LogWarning($"{_speaker.name}'s primary clip is null, so it will skipped.");
                }
                yield break;
            }

            IEnumerator PlaySecondaryClip()
            {
                SfxBase clip;
                if (Random.Range(0f, 1f) < _secondaryChance)
                {
                    clip = _speaker.SecondaryClip;

                    if (clip == null)
                    {
                        Debug.LogWarning($"{_speaker.name}'s secondary clip is null, so it will skipped.");
                        yield break;
                    }
                }
                else
                {
                    clip = _speaker.TertiaryClip;

                    if (clip == null)
                    {
                        Debug.LogWarning($"{_speaker.name}'s tertiary clip is null, so it will skipped.");
                        yield break;
                    }
                }

                float waitTime = PlayClip(clip);
                yield return new WaitForSeconds(waitTime);
                yield break;
            }

            float PlayClip(SfxBase clip)
            {
                _audioSource.SetSourceProperties(clip.GetSourceProperties());
                _audioSource.Play();

                if (_audioSource.Source.clip == null)
                {
                    Debug.LogWarning($"SfxBase object, {clip.name}, has a null clip. Please correct this.");
                    return 0;
                }
                else
                {
                    return _audioSource.Source.clip.length;
                }
            }
        }
    }
}