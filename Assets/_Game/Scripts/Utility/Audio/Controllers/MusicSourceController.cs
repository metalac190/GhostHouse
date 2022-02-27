using System.Collections;
using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Managers;

namespace Utility.Audio.Controllers
{
    public class MusicSourceController : ASC_Base
    {
        private MusicTrack _current;
        private bool _routineActive;
        private float _timeToStop;
        private bool _active;

        private void Update() {
            if (_active && Time.time >= _timeToStop) {
                SoundManager.MusicManager.NextTrack();
                _active = false;
            }
        }

        public void ResetSource() {
            Source.outputAudioMixerGroup = SoundManager.MusicGroup;
        }

        public void PlayMusic(MusicTrack track) {
            _timeToStop = Time.time + track.TrackLength - Mathf.Max(track.FadeOutTime, track.CrossFadeInOverlap);
            _current = track;
            _active = true;
            SetSourceProperties(track.GetSourceProperties());
            StartCoroutine(FadeRoutine(track.FadeInTime, true));
            Play();
        }

        public void PlayMusic(MusicTrack track, float time) {
            if (time < 0 || time >= track.TrackLength) return;
            _current = track;
            _active = true;
            SetSourceProperties(track.GetSourceProperties());
            if (time < track.FadeInTime) {
                StartCoroutine(FadeRoutine(track.FadeInTime, true, time));
            }
            float offsetFromEnd = track.TrackLength - time;
            if (track.FadeOutTime > offsetFromEnd) {
                StartCoroutine(FadeRoutine(track.FadeOutTime, false, track.FadeOutTime - offsetFromEnd));
                _active = false;
            }
            else {
                _timeToStop = Time.time + track.TrackLength - time - Mathf.Max(track.FadeOutTime, track.CrossFadeInOverlap);
            }
            Play();
        }

        public void StopMusic() {
            _active = false;
            StartCoroutine(FadeRoutine(_current.FadeOutTime, false));
        }

        private IEnumerator FadeRoutine(float timer, bool fadeIn, float startTime = 0) {
            if (_routineActive) {
                yield return null;
            }
            _routineActive = true;
            for (float t = startTime; t < timer; t += Time.time) {
                float delta = t / timer;
                Debug.Log(fadeIn + " " + delta);
                SetCustomVolume(_current.Evaluate(delta, fadeIn));
                yield return null;
            }
            _routineActive = false;
            if (!fadeIn) {
                Stop();
            }
        }

        public override void Stop() {
            base.Stop();
            SoundManager.MusicManager.ReturnController(this);
        }
    }
}