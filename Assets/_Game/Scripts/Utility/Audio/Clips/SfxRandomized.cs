using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Utility.Audio.Clips.Base;
using Utility.Audio.Helper;
using Utility.RangedFloats;

namespace Utility.Audio.Clips
{
    [CreateAssetMenu(menuName = "Sound System/Sfx Randomized")]
    public class SfxRandomized : SfxBase
    {
        [Header("Audio Clip Settings")]
        [SerializeField] private List<SfxReference> _clips = new List<SfxReference> { new SfxReference() };
        [SerializeField] private AudioMixerGroup _mixerGroup = null;
        [SerializeField] private SfxPriorityLevel _priority = SfxDefaults.Priority;

        [Header("Volume Settings")]
        [SerializeField, MinMaxRange(SfxDefaults.VolumeMin, SfxDefaults.VolumeMax)]
        private RangedFloat _volume = new RangedFloat(SfxDefaults.Volume);
        [SerializeField, MinMaxRange(SfxDefaults.PitchMin, SfxDefaults.PitchMax)]
        private RangedFloat _pitch = new RangedFloat(SfxDefaults.Pitch);
        [SerializeField, MinMaxRange(SfxDefaults.StereoPanMin, SfxDefaults.StereoPanMax), Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo")]
        private RangedFloat _stereoPan = new RangedFloat(SfxDefaults.StereoPan);
        [SerializeField, MinMaxRange(SfxDefaults.ReverbZoneMixMin, SfxDefaults.ReverbZoneMixMax), Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones")]
        private RangedFloat _reverbZoneMix = new RangedFloat(SfxDefaults.ReverbZoneMix);

        [Header("Spatial Settings")]
        [SerializeField, MinMaxRange(SfxDefaults.SpatialBlendMin, SfxDefaults.SpatialBlendMax),
         Tooltip("Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D")]
        private RangedFloat _spatialBlend = new RangedFloat(SfxDefaults.SpatialBlend);
        [SerializeField, Tooltip("Sets/Gets how the AudioSource attenuates over distance")]
        private AudioRolloffMode _rolloffMode = SfxDefaults.RolloffMode;
        [SerializeField, MinMaxRange(SfxDefaults.MinDistanceMin, SfxDefaults.MinDistanceMax), Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume")]
        private RangedFloat _minDistance = new RangedFloat(SfxDefaults.MinDistance);
        [SerializeField, MinMaxRange(SfxDefaults.MaxDistanceMin, SfxDefaults.MaxDistanceMax), Tooltip("(Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at")]
        private RangedFloat _maxDistance = new RangedFloat(SfxDefaults.MaxDistance);
        [SerializeField, MinMaxRange(SfxDefaults.SpreadMin, SfxDefaults.SpreadMax), Tooltip("Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space")]
        private RangedFloat _spread = new RangedFloat(SfxDefaults.Spread);
        [SerializeField, MinMaxRange(SfxDefaults.DopplerLevelMin, SfxDefaults.DopplerLevelMax), Tooltip("Sets the Doppler scale for this AudioSource")]
        private RangedFloat _dopplerLevel = new RangedFloat(SfxDefaults.DopplerLevel);

        public override SfxProperties GetSourceProperties() {
            // Remove any clips that are null and remove the sfx reference if it is the same as this sfx event (prevent recursion)
            _clips = _clips.Where(clip => clip != null && !clip.NullTest() && !clip.TestSame(this)).ToList();

            // If there are no clips, return an empty reference
            if (_clips.Count == 0) {
                return new SfxProperties(true);
            }

            // Find Reference Source Properties
            var referenceSfx = _clips[Random.Range(0, _clips.Count)];
            var referenceProperties = referenceSfx.GetSourceProperties();

            // Create Current Source Properties
            var myProperties = new SfxProperties(_mixerGroup, (int)_priority, _volume.GetRandom(), _pitch.GetRandom(), _stereoPan.GetRandom(), _reverbZoneMix.GetRandom(), Vector3.zero,
                _spatialBlend.GetRandom(), _rolloffMode, _minDistance.GetRandom(), _maxDistance.GetRandom(), Mathf.RoundToInt(_spread.GetRandom()), _dopplerLevel.GetRandom());

            // Add properties together and return
            return referenceProperties.AddProperties(myProperties);
        }
    }
}