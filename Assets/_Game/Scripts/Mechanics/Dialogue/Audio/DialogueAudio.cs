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
        public bool CanPlay { get; private set; } = false;

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

        SOCharacterAudio _speaker = null;
        NextClip _nextClip = NextClip.None;
        int _indexOfLastWord = -1;
        float _waitLength = 0;
        float _waitAccumulator = 0;
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
                return;
            }
            else if (_characterAudioPool == null)
            {
                Debug.LogWarning($"\"{name}\" is unable to play dialogue audio. No SOCharacterAudioPool was provided to _characterAudioPool.");
                this.enabled = false;
                return;
            }

            _characterView.OnLineStarted += OnLineStart;
            _characterView.OnCharacterTyped += OnLineUpdate;
            _characterView.OnLineEnd += OnLineEnd;
            PauseMenu.PauseUpdated += Pause;
        }

        void OnDisable()
        {
            if (_characterView == null && _characterAudioPool == null) return;

            _characterView.OnLineStarted -= OnLineStart;
            _characterView.OnCharacterTyped -= OnLineUpdate;
            _characterView.OnLineEnd -= OnLineEnd;
            PauseMenu.PauseUpdated -= Pause;
        }

        void Pause(bool paused)
        {
            if (paused)
            {
                _audioSource.Source.Pause();
                CanPlay = false;
            }
            else
            {
                _audioSource.Source.UnPause();
                CanPlay = true;
            }
        }

        void Update()
        {
            if (!CanPlay || _nextClip == NextClip.None) return;

            // increment time
            _waitAccumulator += Time.deltaTime;

            // The current clip is still playing, so wait till it ends
            if (_waitAccumulator < _waitLength) return;

            // play next clip
            switch (_nextClip)
            {
                case NextClip.Primary:
                    PlayClip(_speaker.PrimaryClip);
                    _nextClip = NextClip.Secondary;
                    break;

                case NextClip.Secondary:
                    SfxBase clip = Random.Range(0f, 1f) < _secondaryChance ? _speaker.SecondaryClip : _speaker.TertiaryClip;
                    PlayClip(clip);
                    _nextClip = NextClip.Primary;
                    break;

                case NextClip.Quaternary:
                    PlayClip(_speaker.QuaternaryClip);
                    _nextClip = NextClip.None;
                    CanPlay = false;
                    break;

                case NextClip.None:
                    CanPlay = false;
                    return;

                default:
                    Debug.LogError("Dialog Audio is an unknown state.");
                    CanPlay = false;
                    _audioSource.Stop();
                    return;
            }

            _waitLength = _audioSource.Source.clip == null ? 0 : _audioSource.Source.clip.length;
            _waitAccumulator = 0f;

        }
        #endregion

        /// <summary>
        /// Begin audio loop.
        /// </summary>
        /// <param name="line"> Line of dialogue being played. </param>
        void OnLineStart(Yarn.Unity.LocalizedLine line)
        {
            CanPlay = true;
            _speaker = _characterAudioPool.GetCharacter(line.CharacterName);
            _nextClip = NextClip.Primary;
            _waitLength = 0;
            _waitAccumulator = float.MaxValue;

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
            }
        }

        /// <summary>
        /// Triggered each time the dialogue display is updated. Tracks when audio loop should transition to final state.
        /// </summary>
        /// <param name="index"></param>
        void OnLineUpdate(int index)
        {
            if (index > _indexOfLastWord && _nextClip != NextClip.Quaternary)
            {
                _nextClip = NextClip.Quaternary;
            }
        }

        /// <summary>
        /// Cleans up currently running coroutines.
        /// </summary>
        void OnLineEnd()
        {
            CanPlay = false;
            _nextClip = NextClip.None;
        }

        void PlayClip(SfxBase clip)
        {
            _audioSource.SetSourceProperties(clip.GetSourceProperties());
            _audioSource.Play();

            if (_audioSource.Source.clip == null)
            {
                Debug.LogWarning($"SfxBase object, {clip.name}, has a null clip. Please correct this.");
            }
        }

        enum NextClip
        {
            None, Primary, Secondary, Quaternary
        }
    }
}