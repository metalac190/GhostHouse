using UnityEngine;
using UnityEngine.Audio;

namespace Utility.Audio.Managers
{
    public class AudioMixerController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer = null;

        [Header("Master Slider")]
        [SerializeField] private string _masterVolume = "MasterVolume";
        [SerializeField] private float _masterStartValue = 1f;

        [Header("Other Sliders")]
        [SerializeField] private string _musicVolume = "MusicVolume";
        [SerializeField] private string _sfxVolume = "SfxVolume";
        [SerializeField] private float _startValue = 0.75f;

        private bool _missingMixer;

        private void Start() {
            SetMasterVolume(_masterStartValue);
            SetMusicVolume(_startValue);
            SetSfxVolume(_startValue);
        }

        private void OnValidate() {
            _missingMixer = _mixer == null;
        }

        public void SetMasterVolume(float volume) {
            if (_missingMixer) return;
            if (volume > 0) {
                volume = Mathf.Log10(volume) * 20;
            }
            else {
                volume = -80;
            }
            _mixer.SetFloat(_masterVolume, volume);
        }

        public void SetMusicVolume(float volume) {
            if (_missingMixer) return;
            if (volume > 0) {
                volume = Mathf.Log10(volume) * 20;
            }
            else {
                volume = -80;
            }
            _mixer.SetFloat(_musicVolume, volume);
        }

        public void SetSfxVolume(float volume) {
            if (_missingMixer) return;
            if (volume > 0) {
                volume = Mathf.Log10(volume) * 20;
            }
            else {
                volume = -80;
            }
            _mixer.SetFloat(_sfxVolume, volume);
        }
    }
}