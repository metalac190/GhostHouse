using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Audio.Clips.Base;
using Utility.Audio.Controllers;

namespace Mechanics.Dialog
{
    [RequireComponent(typeof(AudioSourceController))]
    public class DialogueAudio : MonoBehaviour
    {
        public static DialogueAudio Instance = null;

        public bool CanPlay { get; private set; } = false;

        #region private variables
        [SerializeField] private bool _debug = false;

        [SerializeField]
        [Tooltip("After playing the primary audio clip, this is the chance of the secondary clip playing and 1-this is the chance of the tertiary clip playing.")]
        float _secondaryChance = .6f;

        [SerializeField]
        SOCharacterAudioPool _characterAudioPool = null;

        [SerializeField]
        [Tooltip("")]
        List<Playable> _toggleableSfx = new List<Playable>();

        CharacterView _characterView
        {
            get
            {
                if (_characterViewInstance == null)
                {
                    _characterViewInstance = FindObjectOfType<CharacterView>();
                }

                return _characterViewInstance;
            }
        }
        CharacterView _characterViewInstance = null;

        AudioSourceController _audioSource = null;

        SOCharacterAudio _speaker = null;
        List<TimedEffect> _timedEffects = new List<TimedEffect>();
        int _indexOfLastWord = -1;
        int _currentIndex;

        NextClip _nextClip = NextClip.None;
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
            _characterView.OnLineInterrupted += OnLineInterrupted;
            _characterView.OnLineEnd += OnLineEnd;
            PauseMenu.PauseUpdated += Pause;
        }

        void OnDisable()
        {
            if (_characterView == null && _characterAudioPool == null) return;

            if (_characterView != null)
            {
                _characterView.OnLineStarted -= OnLineStart;
                _characterView.OnCharacterTyped -= OnLineUpdate;
                _characterView.OnLineInterrupted -= OnLineInterrupted;
                _characterView.OnLineEnd -= OnLineEnd;
            }

            PauseMenu.PauseUpdated -= Pause;
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

        /// <summary>
        /// Begin audio loop.
        /// </summary>
        /// <param name="line"> Line of dialogue being played. </param>
        void OnLineStart(Yarn.Unity.LocalizedLine line)
        {
            if (_debug) Debug.Log("Dialogue Started");
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

            _currentIndex = 0;
            _indexOfLastWord = line.TextWithoutCharacterName.Text.TrimEnd().LastIndexOf(" ");
            if (_indexOfLastWord < 0)
            {
                //Debug.LogWarning("Unable to find the starting index of the last word. This line's dialog audio will not end with the quaternary clip.");
                _indexOfLastWord = line.TextWithoutCharacterName.Text.Length;
            }

            if (_speaker == null)
            {
                Debug.LogWarning($"Unable to find character \"{line.CharacterName}\". Dialog audio will not be played for this line.");
            }

            _timedEffects = new List<TimedEffect>();
            int colonPosition = line.Text.Text.IndexOf(':') + 1;
            foreach (var attrib in line.Text.Attributes)
            {
                if (attrib.Name == "sfx")
                {
                    _timedEffects.Add(new TimedEffect(
                        attrib.Properties["sfx"].StringValue,
                        attrib.Properties["active"].BoolValue,
                        attrib.Position - colonPosition // Jank fix by Brandon, plz no more bugs <3
                    ));
                }
            }
        }

        /// <summary>
        /// Triggered each time the dialogue display is updated. Tracks when audio loop should transition to final state.
        /// </summary>
        /// <param name="index"></param>
        void OnLineUpdate(int index)
        {
            _currentIndex = index;
            if (index > _indexOfLastWord && _nextClip != NextClip.Quaternary)
            {
                _nextClip = NextClip.Quaternary;
            }

            //if (_debug && _timedEffects.Count > 0) Debug.Log("Toggleable Sfx Available: " + _timedEffects.Count);
            for (int i = _timedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _timedEffects[i];
                if (_debug) Debug.Log("Check Toggleable Sfx (" + effect.Identifer + ", index: " + effect.Index +  ") at index " + index);
                if (index >= effect.Index)
                {
                    //if (_debug) Debug.Log("Toggleable Sfx Found");
                    // toggle effect
                    foreach (var toggleable in _toggleableSfx)
                    {
                        //if (_debug) Debug.Log("Comparing (" + toggleable.Identifer + ") and (" + effect.Identifer + ")");
                        if (!toggleable.Identifer.ToLower().Equals(effect.Identifer.ToLower())) continue;

                        if (effect.Active)
                        {
                            if (_debug) Debug.Log("Start Toggleable Sfx: " + effect.Identifer);
                            toggleable.Controller.Enable();
                            toggleable.Controller.Play();
                        }
                        else
                        {
                            if (_debug) Debug.Log("Stop Toggleable Sfx: " + effect.Identifer);
                            toggleable.Controller.Disable();
                        }
                    }

                    _timedEffects.RemoveAt(i);
                }
            }
        }

        void OnLineInterrupted()
        {
            for (int i = _timedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _timedEffects[i];
                if (_debug) Debug.Log("Check Interrupted Toggleable Sfx (" + effect.Identifer + ") after index " + _currentIndex);
                if (_currentIndex < effect.Index)
                {
                    foreach (var toggleable in _toggleableSfx)
                    {
                        if (!toggleable.Identifer.ToLower().Equals(effect.Identifer.ToLower())) continue;

                        if (effect.Active)
                        {
                            if (_debug) Debug.Log("Force Start Toggleable Sfx: " + effect.Identifer);
                            toggleable.Controller.Enable();
                            toggleable.Controller.Play();
                        }
                        else
                        {
                            if (_debug) Debug.Log("Force Stop Toggleable Sfx: " + effect.Identifer);
                            toggleable.Controller.Disable();
                        }
                    }

                    _timedEffects.RemoveAt(i);
                }
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

        [System.Serializable]
        class Playable
        {
            public string Identifer = string.Empty;
            public AudioSourceController Controller = null;
        }
    }

    public class TimedEffect
    {
        public string Identifer;
        public bool Active;
        public int Index;

        public TimedEffect(string identifer, bool active, int index)
        {
            Identifer = identifer;
            Active = active;
            Index = index;
        }
    }
}