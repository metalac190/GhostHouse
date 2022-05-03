using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Yarn.Unity;

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

        public event Action OnLineInterrupted; 
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

        [SerializeField]
        SOCharacterPool _characterData = null;

        [SerializeField]
        bool _characterNameInLine = false;

        [SerializeField]
        DialogView _leftView = null;

        [SerializeField]
        DialogView _rightView = null;
        #endregion

        #region private variables
        InterruptionFlag _interruptionFlag = new InterruptionFlag();
        Yarn.Unity.LocalizedLine _currentLine = null;

        CanvasGroup _canvasGroup;
        Yarn.Markup.MarkupAttribute _markup;

        DialogView _currentView = null;
        float _lineStartStamp = -1;
        #endregion

        #region Monobehaviour
        public void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_characterData == null)
            {
                Debug.LogWarning($"{name} was not provided _characterData");
            }

            HideView();
        }

        public void Update()
        {
            // update progress bar
            if (_currentView?.Sldr_progressbar != null && _lineStartStamp != -1 && _currentLine != null)
            {
                if (_currentLine.Status == Yarn.Unity.LineStatus.Presenting)
                {
                    float duration = _currentView.Txt_dialog.text.Length / _typewriterEffectSpeed;
                    float elapsedTime = Time.time - _lineStartStamp;
                    _currentView.Sldr_progressbar.value = elapsedTime / duration;
                }
                else if (_currentLine.Status == Yarn.Unity.LineStatus.Interrupted || _currentLine.Status == Yarn.Unity.LineStatus.FinishedPresenting)
                {
                    _currentView.Sldr_progressbar.value = _currentView.Sldr_progressbar.maxValue;
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
                    OnLineInterrupted?.Invoke();
                    _interruptionFlag.Set();
                    break;
                case Yarn.Unity.LineStatus.FinishedPresenting:
                    // The line has finished being delivered by all views.
                    // Display the Continue button.
                    if (_currentView.Btn_continue != null)
                    {
                        _currentView.Btn_continue.SetActive(true);
                        var selectable = _currentView.Btn_continue.GetComponentInChildren<Selectable>();
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

        public override void DialogueStarted()
        {
            _currentView = _rightView;
            _currentView.Txt_characterName.text = String.Empty;
        }

        public override void RunLine(Yarn.Unity.LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _currentLine = dialogueLine;
            SOCharacter character = _characterData.GetCharacter(dialogueLine.CharacterName);

            // flip between dialog views
            if (character != null && character.ShowPortrait)
            {
                // this is not the previous character
                if (_currentView == null || !_currentView.Txt_characterName.text.ToLower().Equals(character.name.ToLower()))
                {
                    if (_currentView == _rightView)
                    {
                        _rightView.gameObject.SetActive(false);
                        _leftView.gameObject.SetActive(true);
                        _currentView = _leftView;
                    }
                    else
                    {
                        _leftView.gameObject.SetActive(false);
                        _rightView.gameObject.SetActive(true);
                        _currentView = _rightView;
                    }
                }
            }

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
            if (_currentView.Img_portrait != null)
            {
                if (_characterData != null)
                {
                    bool characterNotFound = character == null;
                    if (characterNotFound || !character.ShowPortrait)
                    {
                        // hide portrait
                        _currentView.Img_portrait.enabled = false;
                    }
                    else
                    {
                        Sprite characterSprite;

                        // select appropriate sprite
                        bool emotiveSprite = dialogueLine.Text.TryGetAttributeWithName("sprite", out _markup);
                        if (emotiveSprite)
                        {
                            CharacterEmotion emote = SOCharacter.StringToEmotion(_markup.Properties["sprite"].StringValue);
                            characterSprite = character.GetSprite(emote);
                        }
                        else
                        {
                            // use default sprite
                            characterSprite = character.GetSprite(CharacterEmotion.Idle);
                        }

                        // configure portrait
                        if (characterSprite != null)
                        {
                            _currentView.Img_portrait.enabled = true;
                            _currentView.Img_portrait.sprite = characterSprite;
                        }
                        else
                        {
                            _currentView.Img_portrait.enabled = false;
                        }
                    }
                }
                else
                {
                    _currentView.Img_portrait.enabled = false;
                }
            }
            #endregion

            // show the correct dialog box
            #region dialog box style
            Image img_dialog = _currentView.Img_dialog;
            if (img_dialog != null)
            {
                if (character != null && character.UseAlternateBoxStyle)
                {
                    img_dialog.sprite = character.AlternateBoxSprite;
                    img_dialog.color = character.AlternateBoxColor;
                }
                else
                {
                    img_dialog.sprite = _currentView.DefaultBoxSprite;
                    img_dialog.color = _currentView.DefaultBoxColor;
                }
            }
            #endregion

            // show continue button
            if (_currentView.Btn_continue != null)
            {
                _currentView.Btn_continue.SetActive(false);
            }

            _interruptionFlag.Clear();

            // show character name
            if (_currentView.Txt_characterName == null)
            {
                if (_characterNameInLine)
                {
                    _currentView.Txt_dialog.text = dialogueLine.Text.Text;
                }
                else
                {
                    _currentView.Txt_dialog.text = dialogueLine.TextWithoutCharacterName.Text;
                }
            }
            else
            {
                _currentView.P_characterName.SetActive(character != null ? character.ShowName : true);
                if (TextBubbleController.Instance != null) {
                    TextBubbleController.Instance.Disable();
                    TextBubbleController.Instance.SetCharacter(dialogueLine.CharacterName);
                }
                _currentView.Txt_characterName.text = dialogueLine.CharacterName;
                _currentView.Txt_dialog.text = dialogueLine.TextWithoutCharacterName.Text;
            }

            if (_useFadeEffect)
            {
                if (_useTypewriterEffect)
                {
                    // If we're also using a typewriter effect, ensure that
                    // there are no visible characters so that we don't
                    // fade in on the text fully visible
                    _currentView.Txt_dialog.maxVisibleCharacters = 0;
                }
                else
                {
                    // Ensure that the max visible characters is effectively unlimited.
                    _currentView.Txt_dialog.maxVisibleCharacters = int.MaxValue;
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
                if (character != null && !character.PlayAudio)
                {
                    StartCoroutine(Tweens.SimpleTypewriter(_currentView.Txt_dialog, _typewriterEffectSpeed, interruption: _interruptionFlag,
                    onComplete: () =>
                    {
                        onDialogueLineFinished();
                    }));
                }
                else
                {
                    OnLineStarted?.Invoke(dialogueLine);
                    StartCoroutine(Tweens.SimpleTypewriter(_currentView.Txt_dialog, _typewriterEffectSpeed, OnCharacterTyped, interruption: _interruptionFlag,
                    onComplete: () =>
                    {
                        OnLineEnd?.Invoke();
                        onDialogueLineFinished();
                    }));
                }
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

        public void OnContinueClicked() {
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

            _leftView.gameObject?.SetActive(false);
            _rightView.gameObject?.SetActive(false);

            // progress bar
            _leftView.Sldr_progressbar.gameObject?.SetActive(false);
            _rightView.Sldr_progressbar.gameObject?.SetActive(false);
            _lineStartStamp = -1;
        }

        /// <summary>
        /// Shows UI references in scene
        /// </summary>
        void ShowView()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;

            _currentView?.gameObject.SetActive(true);

            if (_currentView == _leftView)
            {
                _rightView?.gameObject?.SetActive(false);
            }
            else
            {
                _leftView?.gameObject?.SetActive(false);
            }

            // progress bar
            Slider progressbar = _currentView.Sldr_progressbar;
            if (progressbar != null)
            {
                progressbar.gameObject.SetActive(true);
                progressbar.value = 0;
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