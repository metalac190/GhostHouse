using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace Mechanics.Dialog
{
    /// <summary>
    /// A heavily recycled version of LineView.cs from YarnSpinner 2.0.2
    /// Used for displaying dialogue, character sprites, and playing dialogue sfx
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CharacterView : Yarn.Unity.DialogueViewBase
    {
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
        [Header("Continue Mode")]
        [SerializeField]
        KeyCode _continueKeyCode = KeyCode.None;

        [Header("Effects")]
        [SerializeField]
        float _inBufferTime = .2f;

        [SerializeField]
        bool _useFadeEffect = false;

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

        [Header("Character Styles")]
        [SerializeField]
        string[] _alternateCharacters = new string[0];

        [SerializeField]
        Image _dialogImage = null;

        [SerializeField]
        Sprite _alternateDialogSprite = null;

        [SerializeField]
        Color _alternateDialogColor = Color.white;

        [Header("UI References")]
        [SerializeField]
        TextMeshProUGUI _lineText = null;

        [SerializeField]
        Image _characterPortraitImage = null;

        [SerializeField]
        SOCharacterPool _charactersData = null;

        [SerializeField]
        GameObject _characterNameObject = null;

        [SerializeField]
        TextMeshProUGUI _characterNameText = null;

        [SerializeField]
        bool _characterNameInLine = false;

        [SerializeField]
        GameObject _continueButton = null;

        [SerializeField]
        GameObject _uiParent = null;

        [SerializeField]
        Slider _progressbar = null;
        #endregion

        #region private variables
        InterruptionFlag _interruptionFlag = new InterruptionFlag();
        Yarn.Unity.LocalizedLine _currentLine = null;

        CanvasGroup _canvasGroup;
        Yarn.Markup.MarkupAttribute _markup;

        float _lineStartStamp = -1;
        Sprite _defaultDialogSprite;
        Color _defaultDialogColor;
        #endregion

        #region Monobehaviour
        void Awake()
        {
            if (_dialogImage != null)
            {
                _defaultDialogSprite = _dialogImage.sprite;
                _defaultDialogColor = _dialogImage.color;
            }
        }

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
            // update progress bar
            if (_progressbar != null && _lineStartStamp != -1 && _currentLine != null)
            {
                if (_currentLine.Status == Yarn.Unity.LineStatus.Presenting)
                {
                    float duration = _lineText.text.Length / _typewriterEffectSpeed;
                    float elapsedTime = Time.time - _lineStartStamp;
                    _progressbar.value = elapsedTime / duration;
                }
                else if (_currentLine.Status == Yarn.Unity.LineStatus.Interrupted || _currentLine.Status == Yarn.Unity.LineStatus.FinishedPresenting)
                {
                    _progressbar.value = _progressbar.maxValue;
                }
            }

            // Should we indicate to the DialogueRunner that we want to
            // interrupt/continue a line? We need to pass a number of
            // checks.

            // We need to be configured to use a keycode to interrupt/continue
            // lines.
            if (_continueKeyCode == KeyCode.None)
            {
                return;
            }

            // That keycode needs to have been pressed this frame.
            if (!Input.GetKeyDown(_continueKeyCode))
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
            if (dialogueLine == null) return;
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
            bool isAlternateCharacter = _alternateCharacters.Any(character => character.ToLower() == dialogueLine.CharacterName.ToLower());

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
                        if (!isAlternateCharacter)
                        {
                            Debug.LogWarning($"Unable to find character \"{dialogueLine.CharacterName}\".");
                        }

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
                            characterSprite = character.GetSprite(CharacterEmotion.Idle);
                        }

                        if (characterSprite != null)
                        {
                            _characterPortraitImage.enabled = true;
                            _characterPortraitImage.sprite = characterSprite;
                        }
                        else
                        {
                            _characterPortraitImage.enabled = false;
                        }
                    }
                }
            }
            #endregion

            // show the correct dialog box
            #region dialog box style
            if (_dialogImage != null)
            {
                if (isAlternateCharacter)
                {
                    _dialogImage.sprite = _alternateDialogSprite;
                    _dialogImage.color = _alternateDialogColor;
                }
                else
                {
                    _dialogImage.sprite = _defaultDialogSprite;
                    _dialogImage.color = _defaultDialogColor;
                }
            }
            #endregion

            // show continue button
            if (_continueButton != null)
            {
                _continueButton.SetActive(false);
            }

            _interruptionFlag.Clear();

            // show character name
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
                // alternate characters do not show their name
                if (isAlternateCharacter)
                {
                    _characterNameObject.SetActive(false);
                }
                // show the name
                else
                {
                    _characterNameObject.SetActive(true);
                }

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

                StartCoroutine(Tweens.LerpAlpha(_canvasGroup, 0, 1, _fadeInTime, interruption: _interruptionFlag,
                onComplete: () =>
                {
                    StartCoroutine(Tweens.WaitBefore(_inBufferTime, () => InEffectComplete()));
                }));
            }
            else
            {
                if (_useTypewriterEffect)
                {
                    StartTypewriter();
                }
                else
                {
                    // no effects were used, so just display the view
                    onDialogueLineFinished();
                }
            }

            // called when the in-animation is complete
            void InEffectComplete()
            {
                if (_useTypewriterEffect)
                {
                    StartTypewriter();
                }
                else
                {
                    onDialogueLineFinished();
                }
            }

            void StartTypewriter()
            {
                OnLineStarted(dialogueLine);
                StartCoroutine(Tweens.SimpleTypewriter(_lineText, _typewriterEffectSpeed, OnCharacterTyped, interruption: _interruptionFlag,
                onComplete: () =>
                {
                    OnLineEnd?.Invoke();
                    onDialogueLineFinished();
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

            // progress bar
            if (_progressbar != null)
            {
                _progressbar.gameObject.SetActive(false);
                _lineStartStamp = -1;
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

            // progress bar
            if (_progressbar != null)
            {
                _progressbar.gameObject.SetActive(true);
                _progressbar.value = 0;
                _lineStartStamp = Time.time;

                if (_useFadeEffect) _lineStartStamp += _fadeInTime + _inBufferTime;
            }
        }

        enum Direction
        {
            Bottom, Left, Top, Right
        }
    }
}