using UnityEngine;
using Utility.Audio.Controllers;
using Utility.Audio.Managers;

namespace Utility.Audio.Helper
{
    public static class AudioHelper
    {
        // Play the clip with given Sfx Property
        public static SfxPoolAudioSource PlayClip(SfxProperties properties, Vector3 position = default) {
            if (properties.Null) return null;
            var controller = PoolController();
            properties.Position = position;
            controller.SetSourceProperties(properties);
            controller.Play();
            return controller;
        }

        // Borrow a Controller from the Sfx Pool
        private static SfxPoolAudioSource PoolController() {
            var controller = SoundManager.Instance.GetController();
            controller.Claimed = true;
            return controller;
        }
    }
}