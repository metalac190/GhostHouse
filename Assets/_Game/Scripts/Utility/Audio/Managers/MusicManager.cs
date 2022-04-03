using System.Collections;
using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Controllers;
using Utility.ObjectPooling;

namespace Utility.Audio.Managers
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private float _pauseTransitionTime = 1;

        private MusicTrack _currentTrack;

        private PoolManager<MusicSourceController> _poolManager = new PoolManager<MusicSourceController>();
        private MusicSourceController _currentController;
        private MusicSourceController _currentPauseController;

        private bool _playingTrack = false;
        private float _timeToLoop;
        private bool _paused;
        private float _pausedStatus;
        private Coroutine _pausedRoutine;

        // Equivalent to Awake / Start (Called from SoundManager)
        public void Setup() {
            _poolManager.BuildInitialPool(transform, SoundManager.DefaultMusicPlayerName, 4);
        }

        private void Update() {
            if (_playingTrack && Time.time >= _timeToLoop) {
                NextTrack();
            }
        }

        public void PlayMusic(MusicTrack musicClip) {
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
            _currentPauseController.PlayMusic(musicClip, delay);
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
                _currentController.SetCustomVolume(1 - _pausedStatus);
            }
            if (_currentPauseController != null) {
                _currentPauseController.SetCustomVolume(_pausedStatus);
            }
        }

        public void NextTrack() {
            PlayMusic(_currentTrack);
        }

        public void ReturnController(MusicSourceController source) {
            source.ResetSource();
            _poolManager.ReturnObject(source);
        }
    }
}