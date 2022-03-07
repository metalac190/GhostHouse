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
        [SerializeField] private AudioClip _trackWhenPaused;

        [Header("Volume Settings")]
        [SerializeField] private AudioMixerGroup _mixerGroup = null;
        [SerializeField] private SfxPriorityLevel _priority = SfxPriorityLevel.Highest;
        [SerializeField, Range(0, 1)] private float _volume = 1;

        [Header("Fade Settings")]
        [SerializeField, ReadOnly] private float _totalClipTime;
        [SerializeField] private float _fadeInTime = 0;
        [SerializeField] private AnimationCurve _fadeIn = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float _fadeOutTime = 0;
        [SerializeField] private float _crossFadeInOverlap = 0;
        [SerializeField] private AnimationCurve _fadeOut = AnimationCurve.Linear(0, 1, 1, 0);

        public bool TrackIsNull => _track == null;
        public float TrackLength => _track.length;
        public float FadeInTime => _fadeInTime;
        public float FadeOutTime => _fadeOutTime;
        public float CrossFadeInOverlap => _crossFadeInOverlap;

        private void OnValidate() {
            _totalClipTime = _track != null ? _track.length : 0;
        }

        public override SfxProperties GetSourceProperties() {
            if (TrackIsNull) return new SfxProperties(true);
            var p = new SfxProperties(_track, _mixerGroup, (int)_priority, _volume, 1, 0, 1, Vector3.zero, false,
                0, AudioRolloffMode.Linear, 100, 100, 0, 1);
            return p;
        }

        public float Evaluate(float delta, bool fadeIn) {
            return Mathf.Clamp01(fadeIn ? _fadeIn.Evaluate(delta) : _fadeOut.Evaluate(delta));
        }
    }
}