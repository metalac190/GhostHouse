using UnityEngine;
using UnityEngine.Audio;

namespace Utility.Audio.Helper
{
    public struct SfxProperties
    {
        // Audio Clip Settings
        public bool Null;
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public int Priority;

        // Volume Settings
        public float Volume;
        public float Pitch;
        public float StereoPan;
        public float ReverbZoneMix;

        // Spatial Settings
        public Vector3 Position;
        public float SpatialBlend;
        public AudioRolloffMode RolloffMode;
        public float MinDistance;
        public float MaxDistance;
        public int Spread;
        public float DopplerLevel;

        public SfxProperties AddProperties(SfxProperties other) {
            // Audio Clip Settings
            if (Clip == null) Clip = other.Clip;
            if (Clip != null) Null = false;
            if (MixerGroup == null) MixerGroup = other.MixerGroup;
            if (Priority != other.Priority) Priority = Mathf.CeilToInt((Priority + other.Priority) * 0.5f);

            // Volume Settings
            Volume *= other.Volume;
            Pitch *= other.Pitch;
            StereoPan += other.StereoPan;
            ReverbZoneMix *= other.ReverbZoneMix;

            // Spatial Settings
            SpatialBlend = (SpatialBlend + other.SpatialBlend) * 0.5f;
            if (RolloffMode == SfxDefaults.RolloffMode) RolloffMode = other.RolloffMode;
            MinDistance = (MinDistance + other.MinDistance) * 0.5f;
            MaxDistance = (MaxDistance + other.MaxDistance) * 0.5f;
            Spread = Mathf.CeilToInt((Spread + other.Spread) * 0.5f);
            DopplerLevel = (DopplerLevel + other.DopplerLevel) * 0.5f;

            return this;
        }

        public SfxProperties(bool _) {
            Null = true;
            Clip = null;
            MixerGroup = null;
            Priority = (int)SfxDefaults.Priority;

            Volume = SfxDefaults.Volume;
            Pitch = SfxDefaults.Pitch;
            StereoPan = SfxDefaults.StereoPan;
            ReverbZoneMix = SfxDefaults.ReverbZoneMix;

            Position = Vector3.zero;
            SpatialBlend = SfxDefaults.SpatialBlend;
            RolloffMode = SfxDefaults.RolloffMode;
            MinDistance = SfxDefaults.MinDistance;
            MaxDistance = SfxDefaults.MaxDistance;
            Spread = SfxDefaults.Spread;
            DopplerLevel = SfxDefaults.DopplerLevel;
        }

        public SfxProperties(AudioClip clip) {
            if (clip == null) {
                Null = true;
                Clip = null;
            }
            else {
                Null = false;
                Clip = clip;
            }
            MixerGroup = null;
            Priority = (int)SfxDefaults.Priority;

            Volume = SfxDefaults.Volume;
            Pitch = SfxDefaults.Pitch;
            StereoPan = SfxDefaults.StereoPan;
            ReverbZoneMix = SfxDefaults.ReverbZoneMix;

            Position = Vector3.zero;
            SpatialBlend = SfxDefaults.SpatialBlend;
            RolloffMode = SfxDefaults.RolloffMode;
            MinDistance = SfxDefaults.MinDistance;
            MaxDistance = SfxDefaults.MaxDistance;
            Spread = SfxDefaults.Spread;
            DopplerLevel = SfxDefaults.DopplerLevel;
        }

        public SfxProperties(AudioMixerGroup mixerGroup, int priority, float volume, float pitch, float stereoPan, float reverbMix, Vector3 position, float spatialBlend,
            AudioRolloffMode rolloffMode, float minDistance, float maxDistance, int spread, float dopplerLevel) {
            Null = true;
            Clip = null;
            MixerGroup = mixerGroup;
            Priority = priority;

            Volume = volume;
            Pitch = pitch;
            StereoPan = stereoPan;
            ReverbZoneMix = reverbMix;

            Position = position;
            SpatialBlend = spatialBlend;
            RolloffMode = rolloffMode;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
            Spread = spread;
            DopplerLevel = dopplerLevel;
        }

        public SfxProperties(AudioClip clip, AudioMixerGroup mixerGroup, int priority, float volume, float pitch, float stereoPan, float reverbMix, Vector3 position, float spatialBlend,
            AudioRolloffMode rolloffMode, float minDistance, float maxDistance, int spread, float dopplerLevel) {
            Null = clip != null;
            Clip = clip;
            MixerGroup = mixerGroup;
            Priority = priority;

            Volume = volume;
            Pitch = pitch;
            StereoPan = stereoPan;
            ReverbZoneMix = reverbMix;

            Position = position;
            SpatialBlend = spatialBlend;
            RolloffMode = rolloffMode;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
            Spread = spread;
            DopplerLevel = dopplerLevel;
        }
    }
}