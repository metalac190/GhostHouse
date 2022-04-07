using UnityEngine;
using UnityEngine.Audio;
using Utility.Audio.Clips.Base;
using Utility.Audio.Helper;
using Utility.ReadOnly;

namespace Utility.Audio.Clips
{
    [CreateAssetMenu(menuName = "Sound System/Music Track")]
    public class MusicTrack : SfxBase
    {
        [Header("Music Track Settings")]
        [SerializeField] private AudioClip _track = null;
        [SerializeField, ReadOnly] private float _trackTime;
        [SerializeField, Range(0, 1)] private float _volume = 1;
        [SerializeField] private AudioMixerGroup _mixerGroup = null;

        [Header("Volume Settings")]
        [SerializeField] private AudioClip _trackWhenPaused = null;
        [SerializeField, ReadOnly] private float _pausedTrackTime;
        [SerializeField, Range(0, 1)] private float _pauseVolume = 1;
        [SerializeField] private AudioMixerGroup _pausedMixerGroup = null;

        [Header("Fade Settings")]
        [SerializeField] private float _fadeInTime = 1;
        [SerializeField] private AnimationCurve _fadeIn = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float _fadeOutTime = 1;
        [SerializeField] private float _delayNextSong = 0;
        [SerializeField] private AnimationCurve _fadeOut = AnimationCurve.Linear(0, 1, 1, 0);

        public bool TrackIsNull => _track == null;
        public float TrackLength => _track.length;
        public float FadeInTime => _fadeInTime;
        public float FadeOutTime => _fadeOutTime;
        public float DelayNextSong => _delayNextSong;

        private void OnValidate() {
            _trackTime = _track != null ? _track.length : 0;
            _pausedTrackTime = _trackWhenPaused != null ? _trackWhenPaused.length : 0;
        }

        public override SfxProperties GetSourceProperties() {
            return GetSourceProperties(false);
        }

        public SfxProperties GetSourceProperties(bool pausedTrack) {
            if (TrackIsNull) return new SfxProperties(true);
            var track = pausedTrack && _trackWhenPaused != null ? _trackWhenPaused : _track;
            var mixer = pausedTrack && _pausedMixerGroup != null ? _pausedMixerGroup : _mixerGroup;
            var p = new SfxProperties(track, mixer, (int)SfxPriorityLevel.Highest, pausedTrack ? _pauseVolume : _volume, 1, 0, 1,
                Vector3.zero, false, 0, AudioRolloffMode.Linear, 100, 100, 0, 1);
            return p;
        }

        public float Evaluate(float delta, bool fadeIn) {
            return Mathf.Clamp01(fadeIn ? _fadeIn.Evaluate(delta) : _fadeOut.Evaluate(delta));
        }
    }
}