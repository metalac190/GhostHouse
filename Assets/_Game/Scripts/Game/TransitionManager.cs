using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

namespace Game
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private Image _fadeToBlack = null;
        [SerializeField] private CanvasGroup _titleBanner = null;
        [SerializeField] private GameObject _raycastBlock = null;

        [Header("Spirit Points")]
        [SerializeField] private int _spiritPointsForLevel = 3;
        //[SerializeField] private Integer _spiritPoints = null;

        [Header("On Scene Load")]
        [SerializeField] private bool _fadeIn = true;
        [SerializeField] private float _fadeInTime = 1;
        [SerializeField] private bool _showTitleText = true;
        [SerializeField] private float _titleTextTime = 1;
        [SerializeField] private AnimationCurve _titleTextVisibility = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.25f, 1), new Keyframe(0.75f, 1), new Keyframe(1, 0));
        [SerializeField] private string _dialogueOnStart = "";

        [Header("On Scene End")]
        [SerializeField] private string _nextScene = "MainMenu";
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private float _fadeOutTime = 1;

        private static DialogueRunner _dialogueRunner;
        private static DialogueRunner DialogueRunner {
            get {
                if (_dialogueRunner == null) {
                    _dialogueRunner = FindObjectOfType<DialogueRunner>();
                }
                return _dialogueRunner;
            }
        }

        private void Start() {
            // Set Spirit Points
            DataManager.Instance.remainingSpiritPoints = _spiritPointsForLevel;
            //if (_spiritPoints != null) _spiritPoints.value = _spiritPointsForLevel;

            // Intro Sequence
            if (_raycastBlock != null) _raycastBlock.gameObject.SetActive(true);
            if (_fadeIn) {
                FadeFromBlack();
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
            if (_fadeToBlack == null) {
                if (_showTitleText) TitleText();
                else StartDialogue();
                return;
            }
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
            else StartDialogue();
        }

        private void TitleText() {
            if (_titleBanner == null) {
                StartDialogue();
                return;
            }
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
            StartDialogue();
        }

        private void StartDialogue() {
            _raycastBlock.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(_dialogueOnStart)) DialogueRunner.StartDialogue(_dialogueOnStart);
        }

        private IEnumerator FadeToBlack(float time) {
            if (_raycastBlock != null) _fadeToBlack.gameObject.SetActive(true);
            for (float t = 0; t < time; t += Time.deltaTime) {
                float delta = t / time;
                _fadeToBlack.color = new Color(0, 0, 0, delta);
                yield return null;
            }
            NextScene();
        }

        private void NextScene() {
            DataManager.Instance.level = _nextScene;
            DataManager.Instance.WriteFile();
            SceneManager.LoadScene(_nextScene);
        }
    }
}