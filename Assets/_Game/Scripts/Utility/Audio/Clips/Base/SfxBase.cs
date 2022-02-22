using UnityEngine;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;

namespace Utility.Audio.Clips.Base
{
    public abstract class SfxBase : ScriptableObject
    {
        public void Play() {
            var sourceProperties = GetSourceProperties();
            AudioHelper.PlayClip(sourceProperties);
        }

        public void Play(ASC_Base controller) {
            var sourceProperties = GetSourceProperties();
            controller.SetSourceProperties(sourceProperties);
            controller.Play();
        }

        public abstract SfxProperties GetSourceProperties();
    }
}