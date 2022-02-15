using UnityEngine;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;

namespace Utility.Audio.Controllers
{
    public class AudioSourceController : ASC_Base
    {
        [SerializeField] private SfxReference _sfx = new SfxReference();
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _looping = true;

        private void Start() {
            if (_sfx != null && _playOnStart) {
                SetSourceProperties(_sfx.GetSourceProperties());
                Source.loop = _looping;
                Play();
            }
        }
    }
}