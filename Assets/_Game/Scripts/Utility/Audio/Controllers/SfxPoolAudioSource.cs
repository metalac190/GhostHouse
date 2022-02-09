using UnityEngine;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Managers;

namespace Utility.Audio.Controllers
{
    public class SfxPoolAudioSource : ASC_Base
    {
        public bool Claimed { get; set; }

        private void LateUpdate() {
            if (!Claimed) return;
            if (Source.isPlaying == false) {
                Stop();
            }
        }

        public void ResetSource() {
            transform.localPosition = Vector3.zero;
            Claimed = false;
            Source.outputAudioMixerGroup = SoundManager.SfxGroup;
        }

        public override void Stop() {
            base.Stop();
            SoundManager.Instance.ReturnController(this);
        }
    }
}