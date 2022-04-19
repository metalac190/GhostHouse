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
    public void FadeInOut() {
        FadeInOut(_fadeOutTime, _fadeHoldTime, _fadeInTime);
    }
    public void FadeInOut(float fadeOutTime, float fadeHoldTime, float fadeInTime) {
        StopAllCoroutines();
        StartCoroutine(FadeToBlack(fadeOutTime, true, fadeHoldTime, fadeInTime));
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void FadeOut() {
        FadeOut(_fadeOutTime);
    }
    public void FadeOut(float fadeOutTime) {
        StopAllCoroutines();
        StartCoroutine(FadeToBlack(fadeOutTime));
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void FadeIn() {
        FadeIn(_fadeInTime);
    }
    public void FadeIn(float fadeInTime) {
        StopAllCoroutines();
        StartCoroutine(FadeFromBlack(fadeInTime));
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
        if (fadeFromAfter) {
            StartCoroutine(FadeHold(waitTime, fadeFromTime));
        }
    }

    private IEnumerator FadeHold(float waitTime, float fadeTime) {
        for (float t = 0; t < waitTime; t += Time.deltaTime) {
            yield return null;
        }
        StartCoroutine(FadeFromBlack(fadeTime));
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
    }

    private void SetImageAlpha(float delta) {
        _image.color = Color.Lerp(_transparent, _color, Mathf.Clamp01(delta));
    }
}
