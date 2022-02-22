using System.Collections;
using UnityEngine;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;
using Utility.RangedFloats;
using Utility.ReadOnly;

namespace Utility.Audio.Controllers
{
    public class AudioSourceController : ASC_Base
    {
        [SerializeField] private SfxReference _sfx = new SfxReference();
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _looping = true;
        [SerializeField, MinMaxRange(0, 100)] private RangedFloat _loopDelay = new RangedFloat(0, 0);
        [SerializeField, ReadOnly] private float _delay;

        private bool _checkLoop;

        private void Start() {
            if (_sfx != null && _playOnStart) {
                SetSourceProperties(_sfx.GetSourceProperties());
                Play();
                if (_looping) {
                    if (_loopDelay.MaxValue > 0) {
                        _checkLoop = _looping;
                    }
                    else {
                        Source.loop = true;
                    }
                }
            }
        }

        private void Update() {
            if (_checkLoop && !Source.isPlaying) {
                _delay = _loopDelay.GetRandom();
                StartCoroutine(LoopDelay());
            }
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
    }
}