using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Controllers;
using Utility.ObjectPooling;

namespace Utility.Audio.Managers
{
    public class MusicManager : MonoBehaviour
    {
        private MusicTrack _currentTrack;

        private PoolManager<MusicSourceController> _poolManager = new PoolManager<MusicSourceController>();
        private MusicSourceController _currentController;


        // Equivalent to Awake / Start (Called from SoundManager)
        public void Setup() {
            _poolManager.BuildInitialPool(transform, SoundManager.DefaultMusicPlayerName, 2);
        }

        public void PlayMusic(MusicTrack musicClip) {
            if (musicClip == null || musicClip.TrackIsNull) {
                return;
            }
            _currentTrack = musicClip;
            if (_currentController != null) {
                _currentController.StopMusic();
            }
            _currentController = _poolManager.GetObject();
            _currentController.PlayMusic(musicClip);
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