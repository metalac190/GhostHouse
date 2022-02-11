using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using Yarn.Markup;

[RequireComponent(typeof(CanvasGroup))]
public class InteractionView : DialogueViewBase
{
    #region private variables
    [Header("UI References")]
    [SerializeField]
    OptionView _optionViewPrefab = null;

    [SerializeField]
    Transform _optionViewParent = null;

    [SerializeField]
    TextMeshProUGUI _lastLineText = null;

    [SerializeField]
    OptionView _cancelButton = null;

    [SerializeField]
    Image _itemImage = null;

    [SerializeField]
    Transform _costParent = null;

    [SerializeField]
    GameObject _costPrefab = null;

    [Header("Effects")]
    [SerializeField]
    float fadeTime = 0.1f;

    [Header("Settings")]
    [SerializeField]
    bool showUnavailableOptions = false;

    // A cached pool of OptionView objects so that we can reuse them
    List<OptionView> optionViews = new List<OptionView>();

    // The method we should call when an option has been selected.
    Action<int> OnOptionSelected;

    // The line we saw most recently.
    LocalizedLine lastSeenLine;

    CanvasGroup _canvasGroup;

    MarkupAttribute _markup;
    #endregion

    public void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        if (_optionViewParent == null)
        {
            Debug.LogWarning("No _optionViewParent was provided. Default value of this.gameObject will be used.");
            _optionViewParent = transform;
        }
    }

    public void Reset()
    {
        _canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        // Don't do anything with this line except note it and
        // immediately indicate that we're finished with it. RunOptions
        // will use it to display the text of the previous line.
        lastSeenLine = dialogueLine;
        onDialogueLineFinished();
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        // Hide all existing option views
        foreach (var optionView in optionViews)
        {
            optionView.gameObject.SetActive(false);
        }

        // If we don't already have enough option views, create more
        while (dialogueOptions.Length > optionViews.Count)
        {
            var optionView = CreateNewOptionView();
            optionView.gameObject.SetActive(false);
        }

        // Set up all of the option views
        int optionViewsCreated = 0;
        DialogueOption cancelOption = null;

        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            var optionView = optionViews[i];
            var option = dialogueOptions[i];

            if (option.IsAvailable == false && showUnavailableOptions == false)
            {
                // Don't show this option.
                continue;
            }

            if (option.Line.Text.TryGetAttributeWithName("cancel", out _markup))
            {
                cancelOption = option;
                continue;
            }

            optionView.gameObject.SetActive(true);

            optionView.Option = option;

            // The first available option is selected by default
            if (optionViewsCreated == 0)
            {
                optionView.Select();
            }

            optionViewsCreated += 1;
        }

        // Update the last line, if one is configured
        if (_lastLineText != null)
        {
            if (lastSeenLine != null) {
                _lastLineText.gameObject.SetActive(true);
                _lastLineText.text = lastSeenLine.Text.Text;
            } else {
                _lastLineText.gameObject.SetActive(false);
            }
        }

        ConfigureInteractionUI(cancelOption);

        // Note the delegate to call when an option is selected
        OnOptionSelected = onOptionSelected;

        // Fade it all in
        StartCoroutine(Effects.FadeAlpha(_canvasGroup, 0, 1, fadeTime));

        /// <summary>
        /// Creates and configures a new <see cref="OptionView"/>, and adds
        /// it to <see cref="optionViews"/>.
        /// </summary>
        OptionView CreateNewOptionView()
        {
            var optionView = Instantiate(_optionViewPrefab);
            optionView.transform.SetParent(_optionViewParent, false);
            optionView.transform.SetAsLastSibling();

            optionView.OnOptionSelected = OptionViewWasSelected;
            optionViews.Add(optionView);

            return optionView;
        }
    }

    /// <summary>
    /// Called by <see cref="OptionView"/> objects.
    /// </summary>
    void OptionViewWasSelected(DialogueOption option)
    {
        StartCoroutine(Effects.FadeAlpha(_canvasGroup, 1, 0, fadeTime, () => OnOptionSelected(option.DialogueOptionID)));
    }

    void ConfigureInteractionUI(DialogueOption option)
    {
        // use lastLine markup to find sprite
        if (lastSeenLine.Text.TryGetAttributeWithName("item_sprite", out _markup))
        {
            MarkupValue val = _markup.Properties["item_sprite"];
            Debug.Log($"Interaction sprite: {val.StringValue}");
        }

        // use lastLine markup to configure spirit point sprites
        if (lastSeenLine.Text.TryGetAttributeWithName("spirit_points", out _markup))
        {
            MarkupValue val = _markup.Properties["spirit_points"];
            Debug.Log($"Interaction Cost: {val.IntegerValue}");
        }

        // enable cancel option
        if (option != null)
        {
            _cancelButton.gameObject.SetActive(true);
            _cancelButton.Option = option;
            _cancelButton.OnOptionSelected = OptionViewWasSelected;
        }
        else
        {
            _cancelButton.gameObject.SetActive(false);
        }
    }
}