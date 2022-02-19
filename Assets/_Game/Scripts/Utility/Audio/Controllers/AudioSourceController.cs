using UnityEngine;
using Utility.Audio.Controllers.Base;
using Utility.Audio.Helper;
using Utility.RangedFloats;

namespace Utility.Audio.Controllers
{
    public class AudioSourceController : ASC_Base
    {
        [SerializeField] private SfxReference _sfx = new SfxReference();
        [SerializeField] private bool _playOnStart = true;
        [SerializeField] private bool _looping = true;
        [SerializeField, MinMaxRange(0, 100)] private RangedFloat _loopDelay = new RangedFloat(0, 0);

        private void Start() {
            if (_sfx != null && _playOnStart) {
                SetSourceProperties(_sfx.GetSourceProperties());
                Source.loop = _looping;
                Play();
            }
        }
    }
}