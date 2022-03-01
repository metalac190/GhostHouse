using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private FadeToBlackAnimation _fade = null;

        [Header("On Scene Load")]
        [SerializeField] private bool _fadeIn = true;
        [SerializeField] private float _fadeInTime = 1;
        [SerializeField] private bool _showTitleText = true;
        [SerializeField] private string _titleText = "";

        [Header("On Scene End")]
        [SerializeField] private string _nextScene = "MainMenu";
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private float _fadeOutTime = 1;

        private void Start() {
            if (_fadeIn) {
                if (_fade != null) _fade.FadeOutUnlocked(_fadeInTime);
            }
            if (_showTitleText) {
                Debug.Log("Title: " + _titleText);
                // Title Text
            }
        }

        public void Transition() {
            if (_fadeOut && _fade != null) {
                _fade.FadeInUnlocked(_fadeInTime);
                StartCoroutine(WaitForFade(_fadeOutTime));
            }
            else {
                NextScene();
            }
        }

        private IEnumerator WaitForFade(float time) {
            yield return new WaitForSecondsRealtime(time);
            NextScene();
        }

        private void NextScene() {
            SceneManager.LoadScene(_nextScene);
        }
    }
}