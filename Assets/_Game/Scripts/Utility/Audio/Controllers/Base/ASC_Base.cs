using UnityEngine;
using Utility.Audio.Helper;

namespace Utility.Audio.Controllers.Base
{
    // Audio Source Controller Base
    [RequireComponent(typeof(AudioSource))]
    public abstract class ASC_Base : MonoBehaviour
    {
        private AudioSource _source;

        internal AudioSource Source {
            get {
                if (_source == null) {
                    _source = GetComponent<AudioSource>();
                    if (_source == null) {
                        _source = gameObject.AddComponent<AudioSource>();
                    }
                }
                return _source;
            }
        }

        public bool IsPlaying => Source.isPlaying;

        private float _originalVolume;

        public void SetSourceProperties(SfxProperties p) {
            var source = Source;
            source.clip = p.Clip;
            if (p.MixerGroup != null) source.outputAudioMixerGroup = p.MixerGroup;
            source.priority = p.Priority;

            source.volume = p.Volume;
            source.pitch = p.Pitch;
            source.panStereo = p.StereoPan;
            source.reverbZoneMix = p.ReverbZoneMix;

            if (p.Position != default) transform.position = p.Position;
            source.spatialBlend = p.SpatialBlend;
            source.rolloffMode = p.RolloffMode;
            source.minDistance = p.MinDistance;
            source.maxDistance = p.MaxDistance;
            source.spread = p.Spread;
            source.dopplerLevel = p.DopplerLevel;

            _originalVolume = p.Volume;
        }

        public void SetCustomVolume(float volume) {
            if (volume < 0) return;
            Source.volume = _originalVolume * volume;
        }

        public void Play() {
            var source = Source;
            if (source.isPlaying) source.Stop();
            source.Play();
        }

        public void Pause() {
            Source.Stop();
        }

        public virtual void Stop() {
            Source.Stop();
        }
    }
}