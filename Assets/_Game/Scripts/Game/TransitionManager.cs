using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private Image _fadeToBlack = null;
        [SerializeField] private CanvasGroup _titleBanner = null;

        [Header("On Scene Load")]
        [SerializeField] private bool _fadeIn = true;
        [SerializeField] private float _fadeInTime = 1;
        [SerializeField] private bool _showTitleText = true;
        [SerializeField] private float _titleTextTime = 1;
        [SerializeField] private AnimationCurve _titleTextVisibility = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 1), new Keyframe(0.75f, 1), new Keyframe(1, 0));

        [Header("On Scene End")]
        [SerializeField] private string _nextScene = "MainMenu";
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private float _fadeOutTime = 1;

        private void Start() {
            if (_fadeIn) {
                FadeFromBlack();
                if (_fadeToBlack != null) StartCoroutine(FadeFromBlack(_fadeInTime));
            }
            else if (_showTitleText) {
                TitleText();
            }
        }

        public void SetNextScene(string scene) {
            _nextScene = scene;
        }

        public void Transition() {
            if (_fadeOut && _fadeToBlack != null) {
                StartCoroutine(FadeToBlack(_fadeOutTime));
            }
            else {
                NextScene();
            }
        }

        private void FadeFromBlack() {
            if (_fadeToBlack != null) return;
            StartCoroutine(FadeFromBlack(_fadeInTime));
        }

        private IEnumerator FadeFromBlack(float time) {
            _fadeToBlack.gameObject.SetActive(true);
            for (float t = 0; t < time; t += Time.deltaTime) {
                float delta = 1 - t / time;
                _fadeToBlack.color = new Color(0, 0, 0, delta);
                yield return null;
            }
            _fadeToBlack.gameObject.SetActive(false);
            if (_showTitleText) TitleText();
        }

        private void TitleText() {
            if (_titleBanner == null) return;
            StartCoroutine(FadeTitleText(_titleTextTime));
        }

        private IEnumerator FadeTitleText(float time) {
            _titleBanner.gameObject.SetActive(true);
            for (float t = 0; t < time; t += Time.deltaTime) {
                float delta = t / time;
                _titleBanner.alpha = _titleTextVisibility.Evaluate(delta);
                yield return null;
            }
            _titleBanner.gameObject.SetActive(false);
        }

        private IEnumerator FadeToBlack(float time) {
            _fadeToBlack.gameObject.SetActive(true);
            for (float t = 0; t < time; t += Time.deltaTime) {
                float delta = t / time;
                _fadeToBlack.color = new Color(0, 0, 0, delta);
                yield return null;
            }
            NextScene();
        }

        private void NextScene() {
            DataManager.Instance.level = _nextScene;
            SceneManager.LoadScene(_nextScene);
        }
    }
}