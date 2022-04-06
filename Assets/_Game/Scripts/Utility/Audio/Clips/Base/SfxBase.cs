using UnityEngine;
using Utility.Audio.Controllers;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;

namespace Utility.Audio.Clips.Base
{
    public abstract class SfxBase : ScriptableObject
    {
        public void Play() => Play(Vector3.zero);
        public SfxPoolAudioSource Play(Vector3 position) {
            var sourceProperties = GetSourceProperties();
            return AudioHelper.PlayClip(sourceProperties, position);
        }

        public void Play(ASC_Base controller) {
            var sourceProperties = GetSourceProperties();
            controller.SetSourceProperties(sourceProperties);
            controller.Play();
        }

        public abstract SfxProperties GetSourceProperties();
    }
}