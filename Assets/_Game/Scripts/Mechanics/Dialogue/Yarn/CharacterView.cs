using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.Dialog
{
    public static class TextEffects
    {
        public static IEnumerator Typewriter(TextMeshProUGUI text, float lettersPerSecond, Action<int> onCharacterTyped = null, Action onComplete = null, Yarn.Unity.InterruptionFlag interruption = null)
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
                    onCharacterTyped?.Invoke(text.maxVisibleCharacters-1);
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

        #region events
        /// <summary>
        /// (LocalizedLine line): line is the dialogue being displayed.
        /// Fires when at the beginning of a line of dialogue if <see cref="_useTypewriterEffect"/> is true.
        /// </summary>
        public event Action<Yarn.Unity.LocalizedLine> OnLineStarted = null;

        /// <summary>
        /// (int index): index is the lastest character shown.
        /// Fires every time a character is typed when <see cref="_useTypewriterEffect"/> is true.
        /// </summary>
        public event Action<int> OnCharacterTyped = null;

        /// <summary>
        /// OnLineEnd.
        /// Fires when at the end of a line of dialogue if <see cref="_useTypewriterEffect"/> is true.
        /// </summary>
        public event Action OnLineEnd = null;
        #endregion

        #region serialized variables
        // CharacterViewEditor depends on serialized variable names
        [Header("Effects")]
        [SerializeField]
        internal bool _useFadeEffect = true;

        [SerializeField]
        [Min(0)]
        float _fadeInTime = 0.25f;

        [SerializeField]
        [Min(0)]
        float _fadeOutTime = 0.05f;

        [SerializeField]
        bool _useTypewriterEffect = false;

        [SerializeField]
        [Min(0)]
        [Tooltip("Typewrite effect speed in characters per second.")]
        float _typewriterEffectSpeed = 120f;

        [Header("UI References")]
        [SerializeField]
        TextMeshProUGUI _lineText = null;

        [SerializeField]
        Image _characterPortraitImage = null;

        [SerializeField]
        SOCharacterPool _charactersData = null;

        [SerializeField]
        TextMeshProUGUI _characterNameText = null;

        [SerializeField]
        bool _characterNameInLine = false;

        [SerializeField]
        GameObject _continueButton = null;

        [SerializeField]
        GameObject _uiParent = null;

        [Header("Continue Mode")]
        [SerializeField]
        ContinueActionType _continueActionType = ContinueActionType.None;

        [SerializeField]
        KeyCode _continueActionKeyCode = KeyCode.Escape;
        #endregion

        #region private variables
        Yarn.Unity.InterruptionFlag _interruptionFlag = new Yarn.Unity.InterruptionFlag();
        Yarn.Unity.LocalizedLine _currentLine = null;

        CanvasGroup _canvasGroup;
        Yarn.Markup.MarkupAttribute _markup;
        #endregion

        #region Monobehaviour
        public void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_characterPortraitImage != null)
            {
                if (_charactersData == null)
                {
                    Debug.LogWarning($"{name} was not provided _charactersData, but a reference to a character image component " +
                        "was provided. The image will be hidden and any requested character sprites will not be shown. Please " +
                        "provide character data if the sprite should be shown.");
                    _characterPortraitImage.enabled = false;
                }
            }

            HideView();
        }

        public void Update()
        {
            // Should we indicate to the DialogueRunner that we want to
            // interrupt/continue a line? We need to pass a number of
            // checks.
            
            // We need to be configured to use a keycode to interrupt/continue
            // lines.
            if (_continueActionType != ContinueActionType.KeyCode)
            {
                return;
            }

            // That keycode needs to have been pressed this frame.
            if (!UnityEngine.Input.GetKeyDown(_continueActionKeyCode))
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
        #endregion

        public override void DismissLine(Action onDismissalComplete)
        {
            _currentLine = null;

            if (_useFadeEffect)
            {
                StartCoroutine(Yarn.Unity.Effects.FadeAlpha(_canvasGroup, 1, 0, _fadeOutTime, onDismissalComplete));
            }
            else
            {
                HideView();
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
                    if (_continueButton != null)
                    {
                        _continueButton.SetActive(true);
                        var selectable = _continueButton.GetComponentInChildren<Selectable>();
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

            #region MARKUP: [interaction/]
            bool skipThisView = dialogueLine.Text.TryGetAttributeWithName("interaction", out _markup);
            if (skipThisView)
            {
                HideView();
                onDialogueLineFinished();
                StartCoroutine(ContinueNextFrame());
                return;
            }
            #endregion

            ShowView();

            // show the character sprite
            #region MARKUP: [sprite=str/]
            if (_characterPortraitImage != null)
            {
                if (_charactersData != null)
                {
                    // find appropriate sprite
                    // configure the ui
                    SOCharacter character = _charactersData.GetCharacter(dialogueLine.CharacterName);

                    if (character == null)
                    {
                        Debug.LogWarning($"Unable to find character \"{dialogueLine.CharacterName}\".");
                        _characterPortraitImage.enabled = false;
                    }
                    else
                    {
                        Sprite characterSprite;

                        bool emotiveSprite = dialogueLine.Text.TryGetAttributeWithName("sprite", out _markup);
                        if (emotiveSprite)
                        {
                            CharacterEmotion emote = SOCharacter.StringToEmotion(_markup.Properties["sprite"].StringValue);
                            characterSprite = character.GetSprite(emote);
                        }
                        else
                        {
                            characterSprite = character.GetSprite(CharacterEmotion.Neutral);
                        }

                        if (characterSprite != null)
                        {
                            _characterPortraitImage.sprite = characterSprite;
                        }
                    }
                }
            }
            #endregion

            if (_continueButton != null)
            {
                _continueButton.SetActive(false);
            }

            _interruptionFlag.Clear();

            if (_characterNameText == null)
            {
                if (_characterNameInLine)
                {
                    _lineText.text = dialogueLine.Text.Text;
                }
                else
                {
                    _lineText.text = dialogueLine.TextWithoutCharacterName.Text;
                }
            }
            else
            {
                _characterNameText.text = dialogueLine.CharacterName;
                _lineText.text = dialogueLine.TextWithoutCharacterName.Text;
            }

            if (_useFadeEffect)
            {
                if (_useTypewriterEffect)
                {
                    // If we're also using a typewriter effect, ensure that
                    // there are no visible characters so that we don't
                    // fade in on the text fully visible
                    _lineText.maxVisibleCharacters = 0;
                }
                else
                {
                    // Ensure that the max visible characters is effectively unlimited.
                    _lineText.maxVisibleCharacters = int.MaxValue;
                }

                // Fade up and then call FadeComplete when done
                StartCoroutine(Yarn.Unity.Effects.FadeAlpha(_canvasGroup, 0, 1, _fadeInTime, () => FadeComplete(onDialogueLineFinished), _interruptionFlag));
            }
            else
            {
                if (_useTypewriterEffect)
                {
                    // Start the typewriter
                    //StartCoroutine(TextEffects.Typewriter(_lineText, _typewriterEffectSpeed, OnCharacterTyped, onDialogueLineFinished, _interruptionFlag));
                    StartTypewriter(onDialogueLineFinished);
                }
                else
                {
                    onDialogueLineFinished();
                }
            }

            void FadeComplete(Action onFinished)
            {
                if (_useTypewriterEffect)
                {
                    //StartCoroutine(TextEffects.Typewriter(_lineText, _typewriterEffectSpeed, OnCharacterTyped, onFinished, _interruptionFlag));
                    StartTypewriter(onFinished);
                }
                else
                {
                    onFinished();
                }
            }

            void StartTypewriter(Action onFinished)
            {
                OnLineStarted(dialogueLine);
                StartCoroutine(TextEffects.Typewriter(_lineText, _typewriterEffectSpeed, OnCharacterTyped, interruption: _interruptionFlag, onComplete: () => {
                    OnLineEnd();
                    onFinished();
                }));
            }
        }

        /// <summary>
        /// Waits till the next frame to call <see cref="OnContinueClicked"/>
        /// </summary>
        IEnumerator ContinueNextFrame()
        {
            yield return null;
            OnContinueClicked();
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

        /// <summary>
        /// Hides UI references in scene
        /// </summary>
        void HideView()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;

            if (_uiParent != null)
            {
                _uiParent.SetActive(false);
            }
        }

        /// <summary>
        /// Shows UI references in scene
        /// </summary>
        void ShowView()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;

            if (_uiParent != null)
            {
                _uiParent.SetActive(true);
            }
        }
    }
}