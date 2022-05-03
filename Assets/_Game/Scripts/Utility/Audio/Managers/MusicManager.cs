using System.Collections;
using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Controllers;
using Utility.ObjectPooling;
using Utility.ReadOnly;

namespace Utility.Audio.Managers
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private bool _debug = false;
        [SerializeField] private float _pauseTransitionTime = 1;

        [SerializeField, ReadOnly] private MusicTrack _currentTrack;

        private PoolManager<MusicSourceController> _poolManager = new PoolManager<MusicSourceController>();
        [SerializeField, ReadOnly] private MusicSourceController _currentController;
        [SerializeField, ReadOnly] private MusicSourceController _currentPauseController;

        [SerializeField, ReadOnly] private bool _playingTrack;
        [SerializeField, ReadOnly] private float _volumeMultiplier = 1;
        [SerializeField, ReadOnly] private float _timeToLoop;
        [SerializeField, ReadOnly] private bool _paused;
        [SerializeField, ReadOnly] private float _pausedStatus;
        private Coroutine _pausedRoutine;
        private Coroutine _multiplyRoutine;

        // Equivalent to Awake / Start (Called from SoundManager)
        public void Setup() {
            _poolManager.BuildInitialPool(transform, SoundManager.DefaultMusicPlayerName, 4);
            _volumeMultiplier = 1;
        }

        private void Update() {
            if (_playingTrack && Time.time >= _timeToLoop) {
                NextTrack();
            }
        }

        public void PlayMusic(MusicTrack musicClip) {
            if (_debug) Debug.Log("Switch active track", gameObject);
            if (musicClip == null || musicClip.TrackIsNull) {
                return;
            }
            _currentTrack = musicClip;
            float delay = 0;
            if (_currentTrack != null) {
                delay = _currentTrack.DelayNextSong;
            }
            StopMusic();
            // Main Track
            _currentController = _poolManager.GetObject();
            _timeToLoop = _currentController.PlayMusic(musicClip, delay);
            // Paused Track
            _currentPauseController = _poolManager.GetObject();
            _currentPauseController.PlayMusic(musicClip, delay, true);
            // Play Tracks
            SetMusicVolume();
            _playingTrack = true;
        }

        public void StopMusic() {
            _playingTrack = false;
            if (_currentController != null) {
                _currentController.StopMusic();
            }
            if (_currentPauseController != null) {
                _currentPauseController.StopMusic();
            }
        }

        public void SetPaused(bool paused) {
            if (_paused == paused) return;
            _paused = paused;
            if (_pausedRoutine != null) {
                StopCoroutine(_pausedRoutine);
            }
            _pausedRoutine = StartCoroutine(PausedRoutine());
        }

        private IEnumerator PausedRoutine() {
            while (true) {
                float delta = Time.deltaTime / _pauseTransitionTime;
                if (_paused) {
                    _pausedStatus += delta;
                    if (_pausedStatus > 1) {
                        _pausedStatus = 1;
                        break;
                    }
                }
                else {
                    _pausedStatus -= delta;
                    if (_pausedStatus < 0) {
                        _pausedStatus = 0;
                        break;
                    }
                }
                SetMusicVolume();
                yield return null;
            }
            SetMusicVolume();
            _pausedRoutine = null;
        }

        private void SetMusicVolume() {
            if (_currentController != null) {
                _currentController.SetMusicVolume((1 - _pausedStatus) * _volumeMultiplier);
            }
            if (_currentPauseController != null) {
                _currentPauseController.SetMusicVolume(_pausedStatus * _volumeMultiplier);
            }
        }

        public void NextTrack() {
            PlayMusic(_currentTrack);
        }

        public void ReturnController(MusicSourceController source) {
            source.ResetSource();
            _poolManager.ReturnObject(source);
        }

        public void SetVolumeMultiplierFade(float multiplier, float time) {
            if (_multiplyRoutine != null) {
                StopCoroutine(_multiplyRoutine);
            }
            _multiplyRoutine = StartCoroutine(SetMultiplier(multiplier, time));
        }

        private IEnumerator SetMultiplier(float newMultiplier, float time) {
            float multiplier = _volumeMultiplier;
            if (_debug) Debug.Log("Set Volume from " + multiplier + " to " + newMultiplier, gameObject);
            for (float t = 0; t < time; t += Time.deltaTime) {
                float delta = t / time;
                _volumeMultiplier = Mathf.Lerp(multiplier, newMultiplier, delta);
                UpdateVolumeMultiplier();
                yield return null;
            }
            _volumeMultiplier = newMultiplier;
            UpdateVolumeMultiplier();
            _multiplyRoutine = null;
        }

        private void UpdateVolumeMultiplier() {
            if (_debug) Debug.Log("Set Faded Volume " + _volumeMultiplier, gameObject);
            if (_pausedRoutine != null) return;
            _currentController.SetMusicVolume(_volumeMultiplier);
        }
    }
}