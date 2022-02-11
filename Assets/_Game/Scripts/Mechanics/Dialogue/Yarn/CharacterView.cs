using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class TextEffects
{
    public static IEnumerator Typewriter(TextMeshProUGUI text, float lettersPerSecond, Action<string, int> onCharacterTyped = null, Action onComplete = null, Yarn.Unity.InterruptionFlag interruption = null)
    {
        // Start with everything invisible
        text.maxVisibleCharacters = 0;

        // Wait a single frame to let the text component process its
        // content, otherwise text.textInfo.characterCount won't be
        // accurate
        yield return null;

        // How many visible characters are present in the text?
        var characterCount = text.textInfo.characterCount;

        // Early out if letter speed is zero or text length is zero
        if (lettersPerSecond <= 0 || characterCount == 0)
        {
            // Show everything and invoke the completion handler
            text.maxVisibleCharacters = characterCount;
            onComplete?.Invoke();
            yield break;
        }

        // Convert 'letters per second' into its inverse
        float secondsPerLetter = 1.0f / lettersPerSecond;

        // If lettersPerSecond is larger than the average framerate, we
        // need to show more than one letter per frame, so simply
        // adding 1 letter every secondsPerLetter won't be good enough
        // (we'd cap out at 1 letter per frame, which could be slower
        // than the user requested.)
        //
        // Instead, we'll accumulate time every frame, and display as
        // many letters in that frame as we need to in order to achieve
        // the requested speed.
        var accumulator = Time.deltaTime;

        while (text.maxVisibleCharacters < characterCount && (interruption == null || interruption.Interrupted == false))
        {
            // We need to show as many letters as we have accumulated
            // time for.
            while (accumulator >= secondsPerLetter)
            {
                text.maxVisibleCharacters += 1;
                onCharacterTyped?.Invoke(text.text, text.maxVisibleCharacters-1);
                accumulator -= secondsPerLetter;
            }
            accumulator += Time.deltaTime;

            yield return null;
        }

        // We either finished displaying everything, or were
        // interrupted. Either way, display everything now.
        text.maxVisibleCharacters = characterCount;

        // Wrap up by invoking our completion handler.
        onComplete?.Invoke();
    }
}

/// <summary>
/// A heavily recycled version of LineView.cs from YarnSpinner 2.0.2
/// Used for displaying dialogue, character sprites, and playing dialogue sfx
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CharacterView : Yarn.Unity.DialogueViewBase
{
    internal enum ContinueActionType
    {
        None,
        KeyCode,
    }

    public static event Action<string, int> OnCharacterTyped = null;

    #region internal variables
    // CharacterViewEditor depends on serialized variable names
    [Header("Effects")]
    [SerializeField]
    internal bool useFadeEffect = true;

    [SerializeField]
    [Min(0)]
    internal float fadeInTime = 0.25f;

    [SerializeField]
    [Min(0)]
    internal float fadeOutTime = 0.05f;

    [SerializeField]
    internal bool useTypewriterEffect = false;

    [SerializeField]
    [Min(0)]
    internal float typewriterEffectSpeed = 120f;

    [Header("UI References")]
    [SerializeField]
    internal TextMeshProUGUI lineText = null;

    [SerializeField]
    internal Image characterPortraitImage = null;

    [SerializeField]
    internal TextMeshProUGUI characterNameText = null;

    [SerializeField]
    internal bool characterNameInLine = false;

    [SerializeField]
    internal GameObject continueButton = null;

    [Header("Continue Mode")]
    [SerializeField]
    internal ContinueActionType continueActionType = ContinueActionType.None;

    [SerializeField]
    internal KeyCode continueActionKeyCode = KeyCode.Escape;

    internal CanvasGroup canvasGroup;
    #endregion

    #region private variables
    Yarn.Unity.InterruptionFlag _interruptionFlag = new Yarn.Unity.InterruptionFlag();
    Yarn.Unity.LocalizedLine _currentLine = null;
    #endregion

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void Update()
    {
        // Should we indicate to the DialogueRunner that we want to
        // interrupt/continue a line? We need to pass a number of
        // checks.
            
        // We need to be configured to use a keycode to interrupt/continue
        // lines.
        if (continueActionType != ContinueActionType.KeyCode)
        {
            return;
        }

        // That keycode needs to have been pressed this frame.
        if (!UnityEngine.Input.GetKeyDown(continueActionKeyCode))
        {
            return;
        }
            
        // The line must not be in the middle of being dismissed.
        if ((_currentLine?.Status) == Yarn.Unity.LineStatus.Dismissed)
        {
            return;
        }

        // We're good to indicate that we want to skip/continue.
        OnContinueClicked();
    }

    public override void DismissLine(Action onDismissalComplete)
    {

        _currentLine = null;

        if (useFadeEffect)
        {
            StartCoroutine(Yarn.Unity.Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime, onDismissalComplete));
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            onDismissalComplete();
        }
    }

    public override void OnLineStatusChanged(Yarn.Unity.LocalizedLine dialogueLine)
    {
        switch (dialogueLine.Status)
        {
            case Yarn.Unity.LineStatus.Presenting:
                break;
            case Yarn.Unity.LineStatus.Interrupted:
                // We have been interrupted. Set our interruption flag,
                // so that any animations get skipped.
                _interruptionFlag.Set();
                break;
            case Yarn.Unity.LineStatus.FinishedPresenting:
                // The line has finished being delivered by all views.
                // Display the Continue button.
                if (continueButton != null)
                {
                    continueButton.SetActive(true);
                    var selectable = continueButton.GetComponentInChildren<Selectable>();
                    if (selectable != null)
                    {
                        selectable.Select();
                    }
                }
                break;
            case Yarn.Unity.LineStatus.Dismissed:
                break;
        }
    }

    public override void RunLine(Yarn.Unity.LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        _currentLine = dialogueLine;

        lineText.gameObject.SetActive(true);
        canvasGroup.gameObject.SetActive(true);

        if (continueButton != null)
        {
            continueButton.SetActive(false);
        }

        _interruptionFlag.Clear();

        if (characterNameText == null)
        {
            if (characterNameInLine)
            {
                lineText.text = dialogueLine.Text.Text;
            }
            else
            {
                lineText.text = dialogueLine.TextWithoutCharacterName.Text;
            }
        }
        else
        {
            characterNameText.text = dialogueLine.CharacterName;
            lineText.text = dialogueLine.TextWithoutCharacterName.Text;
        }

        if (useFadeEffect)
        {
            if (useTypewriterEffect)
            {
                // If we're also using a typewriter effect, ensure that
                // there are no visible characters so that we don't
                // fade in on the text fully visible
                lineText.maxVisibleCharacters = 0;
            }
            else
            {
                // Ensure that the max visible characters is effectively unlimited.
                lineText.maxVisibleCharacters = int.MaxValue;
            }

            // Fade up and then call FadeComplete when done
            StartCoroutine(Yarn.Unity.Effects.FadeAlpha(canvasGroup, 0, 1, fadeInTime, () => FadeComplete(onDialogueLineFinished), _interruptionFlag));
        }
        else
        {
            // Immediately appear 
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

            if (useTypewriterEffect)
            {
                // Start the typewriter
                StartCoroutine(TextEffects.Typewriter(lineText, typewriterEffectSpeed, CharacterView.OnCharacterTyped, onDialogueLineFinished, _interruptionFlag));
            }
            else
            {
                onDialogueLineFinished();
            }
        }

        void FadeComplete(Action onFinished)
        {
            if (useTypewriterEffect)
            {
                StartCoroutine(TextEffects.Typewriter(lineText, typewriterEffectSpeed, CharacterView.OnCharacterTyped, onFinished, _interruptionFlag));
            }
            else
            {
                onFinished();
            }
        }
    }

    public void OnContinueClicked()
    {
        if (_currentLine == null)
        {
            // We're not actually displaying a line. No-op.
            return;
        }
        ReadyForNextLine();
    }
}