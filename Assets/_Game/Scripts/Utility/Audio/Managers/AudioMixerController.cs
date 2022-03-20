using UnityEngine;
using UnityEngine.Audio;
using Utility.Audio.Helper;

namespace Utility.Audio.Managers
{
    public class AudioMixerController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer = null;

        [Header("Master Slider")]
        [SerializeField] private float _masterStartValue = 1f;
        [SerializeField] private float _otherStartValue = 0.75f;

        private bool _missingMixer;

        private void Start() {
            SetMasterVolume(_masterStartValue);
            SetMusicVolume(_otherStartValue);
            SetSfxVolume(_otherStartValue);
            SetAmbienceVolume(_otherStartValue);
            SetDialogueVolume(_otherStartValue);
        }

        private void OnValidate() {
            _missingMixer = _mixer == null;
        }

        public void SetMasterVolume(float volume) => SetVolume(AudioMixerEnum.Master, volume);
        public void SetMusicVolume(float volume) => SetVolume(AudioMixerEnum.Music, volume);
        public void SetSfxVolume(float volume) => SetVolume(AudioMixerEnum.Sfx, volume);
        public void SetAmbienceVolume(float volume) => SetVolume(AudioMixerEnum.Ambience, volume);
        public void SetDialogueVolume(float volume) => SetVolume(AudioMixerEnum.Dialogue, volume);

        public void SetVolume(AudioMixerEnum mixer, float volume) {
            if (_missingMixer) return;
            string mixerParameter = AudioMixerHelper.GetAudioMixerParameter(mixer);
            if (string.IsNullOrEmpty(mixerParameter)) return;
            if (volume > 0) {
                volume = Mathf.Log10(volume) * 20;
            }
            else {
                volume = -80;
            }
            _mixer.SetFloat(mixerParameter, volume);
        }
    }
}