using UnityEngine;
using Utility.Audio.Clips;
using Utility.Audio.Managers;

namespace Utility.Audio.Helper
{
    public class MusicTrigger : MonoBehaviour
    {
        [SerializeField] private MusicTrack _music = null;
        [SerializeField] private bool _playOnStart = true;

        private void Start() {
            if (_playOnStart) {
                PlayMusic();
            }
        }

        public void PlayMusic() {
            SoundManager.MusicManager.PlayMusic(_music);
        }
    }
}