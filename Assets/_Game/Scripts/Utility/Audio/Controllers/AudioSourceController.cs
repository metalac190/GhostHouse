using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;
using Utility.Buttons;
using Utility.RangedFloats;
using Utility.ReadOnly;

namespace Utility.Audio.Controllers
{
    public class AudioSourceController : ASC_Base
    {
        [SerializeField] private bool _enableSounds = true;
        [SerializeField] private SfxReference _sfx = new SfxReference();
        [SerializeField] private AudioMixerGroup _overrideMixer = null;
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _waitOnStart = true;
        [SerializeField] private bool _looping = true;
        [SerializeField, MinMaxRange(0, 100)] private RangedFloat _loopDelay = new RangedFloat(0, 0);
        [SerializeField, ReadOnly] private float _delay;

        private bool _checkLoop;
        private bool _areSoundsEnabled = true;

        private void Start() {
            InitializeSfx();
        }

        private void Update() {
            if (_checkLoop && !Source.isPlaying) {
                Delay();
            }
        }

        private void OnValidate() {
            CheckEnabled();
        }

        public void Enable() {
            _enableSounds = true;
            CheckEnabled();
        }

        public void Disable() {
            _enableSounds = false;
            CheckEnabled();
        }

        private void InitializeSfx() {
            if (_sfx == null) return;

            SetSourceProperties(_sfx.GetSourceProperties());
            if (_overrideMixer != null) {
                Source.outputAudioMixerGroup = _overrideMixer;
            }
            if (_playOnStart) {
                if (_looping) {
                    if (_loopDelay.MaxValue > 0) {
                        Source.loop = false;
                        if (_waitOnStart) {
                            Delay();
                        }
                        else {
                            Play();
                            _checkLoop = true;
                        }
                    }
                    else {
                        Source.loop = true;
                    }
                }
                else {
                    Play();
                }
            }
        }

        private void Delay() {
            _delay = _loopDelay.GetRandom();
            StartCoroutine(LoopDelay());
        }

        private IEnumerator LoopDelay() {
            _checkLoop = false;
            Stop();
            for (float t = 0; t < _delay; t += Time.deltaTime) {
                yield return null;
            }
            Play();
            _checkLoop = true;
        }

        private void CheckEnabled() {
            if (_enableSounds == _areSoundsEnabled) return;
            _areSoundsEnabled = _enableSounds;
            if (_enableSounds) {
                if (_looping) {
                    if (_waitOnStart) {
                        Delay();
                    }
                    else {
                        Play();
                        _checkLoop = true;
                    }
                }
                else {
                    Play();
                }
            }
            else {
                _checkLoop = false;
                Stop();
            }
        }

        [Button(Spacing = 10, Mode = ButtonMode.NotPlaying)]
        private void ForceUpdateSfxProperties() {
            Stop();
            StopAllCoroutines();
            InitializeSfx();
        }
    }
}