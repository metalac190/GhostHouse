using System;
using UnityEngine;
using Utility.Audio.Clips.Base;
using Utility.Audio.Controllers;

namespace Utility.Audio.Helper
{
    [Serializable]
    public class SfxReference
    {
        // Uses a custom editor to allow user to insert an AudioClip or a variant of SfxBase
        public bool UseConstant;
        public AudioClip Clip;
        public SfxBase Base;

        // Default constructor
        public SfxReference() {
            UseConstant = false;
            Clip = null;
        }

        // Play the clip
        public void Play(Vector3 position = default) {
            if (NullTest()) return;
            if (UseConstant) {
                AudioHelper.PlayClip(new SfxProperties(Clip), position);
            }
            else {
                Base.Play(position);
            }
        }

        public void Play(SfxPoolAudioSource source) {
            if (NullTest()) return;
            if (UseConstant) {
                source.SetSourceProperties(new SfxProperties(Clip));
                source.Play();
            }
            else {
                Base.Play(source);
            }
        }

        public SfxProperties GetSourceProperties() {
            if (NullTest()) return new SfxProperties(true);
            return UseConstant ? new SfxProperties(Clip) : Base.GetSourceProperties();
        }

        public bool NullTest() {
            if (UseConstant) {
                return Clip == null;
            }
            return Base == null;
        }

        public bool TestSame(SfxBase other) {
            if (!UseConstant) {
                return Base == other;
            }
            return false;
        }
    }
}