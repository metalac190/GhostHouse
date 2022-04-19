using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility.Buttons;

public class SimpleFadeToBlack : MonoBehaviour
{
    public static SimpleFadeToBlack Singleton;

    [SerializeField] private bool _singleton = true;
    [SerializeField] private Image _image = null;
    [SerializeField] private float _fadeOutTime = 1;
    [SerializeField] private float _fadeHoldTime = 0.2f;
    [SerializeField] private float _fadeInTime = 1;

    private Color _color;
    private Color _transparent;
    private float _delta;
    private Coroutine _currentRoutine;

    private void Awake() {
        if (_singleton) {
            Singleton = this;
        }
    }

    private void Start() {
        var color = _image.color;
        _color = color;
        color.a = 0;
        _transparent = color;
    }

    private void OnValidate() {
        if (_image == null) {
            _image = GetComponent<Image>();
            if (_image == null) {
                _image = gameObject.AddComponent<Image>();
                _image.color = Color.black;
                _image.enabled = false;
            }
        }
    }

    [Button(Spacing = 10, Mode = ButtonMode.NotPlaying)]
    public void FadeOutIn() {
        FadeOutIn(_fadeOutTime, _fadeHoldTime, _fadeInTime);
    }
    public Coroutine FadeOutIn(float fadeOutTime, float fadeHoldTime, float fadeInTime) {
        StopCurrentRoutine();
        _currentRoutine = StartCoroutine(FadeToBlack(fadeOutTime, true, fadeHoldTime, fadeInTime));
        return _currentRoutine;
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void FadeOut() {
        FadeOut(_fadeOutTime);
    }
    public Coroutine FadeOut(float fadeOutTime) {
        StopCurrentRoutine();
        _currentRoutine = StartCoroutine(FadeToBlack(fadeOutTime));
        return _currentRoutine;
    }

    public Coroutine WaitFadeIn(float fadeWaitTime, float fadeInTime) {
        StopCurrentRoutine();
        _currentRoutine = StartCoroutine(FadeHold(fadeWaitTime, fadeInTime));
        return _currentRoutine;
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void FadeIn() {
        FadeIn(_fadeInTime);
    }
    public Coroutine FadeIn(float fadeInTime) {
        StopCurrentRoutine();
        _currentRoutine = StartCoroutine(FadeFromBlack(fadeInTime));
        return _currentRoutine;
    }

    private void StopCurrentRoutine() {
        if (_currentRoutine != null) {
            StopCoroutine(_currentRoutine);
        }
    }

    private IEnumerator FadeToBlack(float time, bool fadeFromAfter = false, float waitTime = 0, float fadeFromTime = 0) {
        _image.enabled = true;
        SetImageAlpha(0);
        float start = _delta * time;
        for (float t = start; t < time; t += Time.deltaTime) {
            _delta = t / time;
            SetImageAlpha(_delta);
            yield return null;
        }
        SetImageAlpha(1);
        _currentRoutine = fadeFromAfter ? StartCoroutine(FadeHold(waitTime, fadeFromTime)) : null;
    }

    private IEnumerator FadeHold(float waitTime, float fadeTime) {
        for (float t = 0; t < waitTime; t += Time.deltaTime) {
            yield return null;
        }
        _currentRoutine = StartCoroutine(FadeFromBlack(fadeTime));
    }

    private IEnumerator FadeFromBlack(float time) {
        SetImageAlpha(1);
        float start = (1 - _delta) * time;
        for (float t = start; t < time; t += Time.deltaTime) {
            _delta = 1 - t / time;
            SetImageAlpha(_delta);
            yield return null;
        }
        SetImageAlpha(0);
        _image.enabled = false;
        _currentRoutine = null;
    }

    private void SetImageAlpha(float delta) {
        _image.color = Color.Lerp(_transparent, _color, Mathf.Clamp01(delta));
    }
}
