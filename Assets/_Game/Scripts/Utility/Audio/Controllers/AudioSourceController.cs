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
        [SerializeField] private bool _debug = false;
        [SerializeField] private SfxReference _sfx = new SfxReference();
        [SerializeField] private AudioMixerGroup _overrideMixer = null;
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _waitOnStart = true;
        [SerializeField] private bool _looping = true;
        [SerializeField, MinMaxRange(0, 100)] private RangedFloat _loopDelay = new RangedFloat(0, 0);
        [SerializeField, ReadOnly] private float _delay;
        [SerializeField, ReadOnly] private bool _isPlaying;

        private bool _checkLoop;
        private bool _areSoundsEnabled;

        private void Awake() {
            Source.playOnAwake = false;
            Source.loop = false;
            Source.Stop();
        }

        private void Start() {
            ResetSfx();
            if (_playOnStart) {
                Enable();
            }
        }

        private void Update() {
            _isPlaying = Source.isPlaying;
            if (_checkLoop && !Source.isPlaying) {
                Delay();
            }
        }

        private void ResetSfx()
        {
            if (_sfx == null) return;
            SetSourceProperties(_sfx.GetSourceProperties());
            if (_overrideMixer != null)
            {
                Source.outputAudioMixerGroup = _overrideMixer;
            }
            _areSoundsEnabled = false;
        }

        [Button(Spacing = 10, Mode = ButtonMode.NotPlaying)]
        private void ForceUpdateSfxProperties()
        {
            Stop();
            StopAllCoroutines();
            ResetSfx();
            CheckEnabled();
        }

        [Button(Spacing = 5, Mode = ButtonMode.NotPlaying)]
        public void Enable() {
            if (_areSoundsEnabled) return;
            _areSoundsEnabled = true;
            CheckEnabled();
        }

        [Button(Mode = ButtonMode.NotPlaying)]
        public void Disable() {
            if (!_areSoundsEnabled) return;
            _areSoundsEnabled = false;
            CheckEnabled();
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
            if (_debug) Debug.Log("Finish Delay");
            Enable();
            _checkLoop = true;
        }

        private void CheckEnabled() {
            if (_areSoundsEnabled && !Source.isPlaying) {
                PlaySource();
            }
            else if (!_areSoundsEnabled) {
                StopSource();
            }
        }

        private void PlaySource() {
            if (_looping) {
                if (_loopDelay.MaxValue > 0) {
                    Source.loop = false;
                    if (_waitOnStart) {
                        Delay();
                        return;
                    }
                    _checkLoop = true;
                }
                else {
                    Source.loop = true;
                }
                Play();
            }
            else {
                if (_debug) Debug.Log("Play Once");
                Source.loop = false;
                Play();
            }
        }

        private void StopSource() {
            _checkLoop = false;
            Stop();
        }
    }
}