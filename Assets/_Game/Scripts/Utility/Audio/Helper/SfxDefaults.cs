using UnityEngine;

namespace Utility.Audio.Helper
{
    public class SfxDefaults
    {
        // Audio Clip Default Settings
        // Clip
        // Mixer Group
        public const SfxPriorityLevel Priority = SfxPriorityLevel.Standard;
        public const bool Loop = false;

        // Volume Settings
        public const float Volume = 1f;
        public const float VolumeMin = 0f;
        public const float VolumeMax = 1f;
        public const float Pitch = 1f;
        public const float PitchMin = 0.25f;
        public const float PitchMax = 3f;
        public const float StereoPan = 0f;
        public const float StereoPanMin = -1f;
        public const float StereoPanMax = 1f;
        public const float ReverbZoneMix = 1f;
        public const float ReverbZoneMixMin = 0f;
        public const float ReverbZoneMixMax = 1.1f;

        // Spatial Settings
        public const float SpatialBlend = 0f;
        public const float SpatialBlendMin = 0f;
        public const float SpatialBlendMax = 1f;
        public const AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
        public const float MinDistance = 0.5f;
        public const float MinDistanceMin = 0.1f;
        public const float MinDistanceMax = 5f;
        public const float MaxDistance = 50f;
        public const float MaxDistanceMin = 5f;
        public const float MaxDistanceMax = 100f;
        public const int Spread = 0;
        public const int SpreadMin = 0;
        public const int SpreadMax = 360;
        public const float DopplerLevel = 1f;
        public const float DopplerLevelMin = 0f;
        public const float DopplerLevelMax = 5f;
    }

    public enum SfxPriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256
    }
}