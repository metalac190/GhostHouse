using Utility.Audio.Controllers;
using Utility.Audio.Managers;

namespace Utility.Audio.Helper
{
    public static class AudioHelper
    {
        // Play the clip with given Sfx Property
        public static void PlayClip(SfxProperties p) {
            if (p.Null) return;
            var controller = PoolController();
            controller.SetSourceProperties(p);
            controller.Play();
        }

        // Borrow a Controller from the Sfx Pool
        private static SfxPoolAudioSource PoolController() {
            var controller = SoundManager.Instance.GetController();
            controller.Claimed = true;
            return controller;
        }
    }
}