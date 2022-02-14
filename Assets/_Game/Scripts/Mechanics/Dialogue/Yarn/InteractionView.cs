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
    #region serialized variables
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
    [Tooltip("Unavailable options will still be shown to user.")]
    bool showUnavailableOptions = false;
    #endregion

    #region private variables
    // A cached pool of OptionView objects so that we can reuse them
    List<OptionView> _optionViews = new List<OptionView>();

    // The method we should call when an option has been selected.
    Action<int> _OnOptionSelected;

    // The line we saw most recently.
    LocalizedLine _lastSeenLine;

    // A cached pool of OptionView objects so that we can reuse them
    List<GameObject> _costInstances = new List<GameObject>();

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
            Debug.LogWarning("No _optionViewParent was provided. Default value of this.transform will be used.");
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
        _lastSeenLine = dialogueLine;
        onDialogueLineFinished();
    }

    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        // Hide all existing option views
        foreach (var optionView in _optionViews)
        {
            optionView.gameObject.SetActive(false);
        }

        // If we don't already have enough option views, create more
        while (dialogueOptions.Length > _optionViews.Count)
        {
            var optionView = CreateNewOptionView();
            optionView.gameObject.SetActive(false);
        }

        // Set up all of the option views
        int optionViewsCreated = 0;
        DialogueOption cancelOption = null;

        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            var optionView = _optionViews[i];
            var option = dialogueOptions[i];

            if (option.IsAvailable == false && showUnavailableOptions == false)
            {
                // Don't show this option.
                continue;
            }

            #region MARKUP: [cancel/]
            if (option.Line.Text.TryGetAttributeWithName("cancel", out _markup))
            {
                cancelOption = option;
                continue;
            }
            #endregion

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
            if (_lastSeenLine != null) {
                _lastLineText.gameObject.SetActive(true);
                _lastLineText.text = _lastSeenLine.Text.Text;
            } else {
                _lastLineText.gameObject.SetActive(false);
            }
        }

        ProcessMarkup(cancelOption);

        // Note the delegate to call when an option is selected
        _OnOptionSelected = onOptionSelected;

        // Fade it all in
        StartCoroutine(Effects.FadeAlpha(_canvasGroup, 0, 1, fadeTime));

        /// <summary>
        /// Creates and configures a new <see cref="OptionView"/>, and adds
        /// it to <see cref="_optionViews"/>.
        /// </summary>
        OptionView CreateNewOptionView()
        {
            var optionView = Instantiate(_optionViewPrefab);
            optionView.transform.SetParent(_optionViewParent, false);
            optionView.transform.SetAsLastSibling();

            optionView.OnOptionSelected = OptionViewWasSelected;
            _optionViews.Add(optionView);

            return optionView;
        }
    }

    /// <summary>
    /// Called by <see cref="OptionView"/> objects.
    /// </summary>
    void OptionViewWasSelected(DialogueOption option)
    {
        StartCoroutine(Effects.FadeAlpha(_canvasGroup, 1, 0, fadeTime, () => _OnOptionSelected(option.DialogueOptionID)));
    }

    /// <summary>
    /// Configures UI according to markup in <see cref="_lastSeenLine"/> and given DialogueOption
    /// </summary>
    /// <param name="option"> DialogueOption containing [cancel/]. Ignored if null. </param>
    void ProcessMarkup(DialogueOption option)
    {
        #region MARKUP: [item_sprite=str]
        if (_lastSeenLine.Text.TryGetAttributeWithName("item_sprite", out _markup))
        {
            ShowItemSprite();
        }
        else
        {
            HideItemSprite();
        }
        #endregion

        #region MARKUP: [spirit_points=int]
        if (_lastSeenLine.Text.TryGetAttributeWithName("spirit_points", out _markup))
        {
            ShowSpiritPointCost();
        }
        else
        {
            HideSpiritPointCost();
        }
        #endregion

        #region MARKUP: [cancel/]
        if (option != null)
        {
            ShowCancelButton(option);
        }
        else
        {
            HideCancelButton();
        }
        #endregion
    }

    /// <summary>
    /// Finds the item sprite requested and displays it
    /// </summary>
    void ShowItemSprite()
    {
        //MarkupValue val = _markup.Properties["item_sprite"];
        //Debug.Log($"Interaction sprite: {val.StringValue}");

        if (_itemImage != null)
        {
            string spriteName = _markup.Properties["item_sprite"].StringValue;
            // find sprite from a manager of somesort

            //if (sprite != null)
            //{
                //_itemImage.gameObject.SetActive(true);
                //_itemImage.sprite = ;
            //}
            //else
            //{
                //Debug.LogError($"Unable to find item sprite: {spriteName}");
            //}
        }
    }

    /// <summary>
    /// Hides the item sprite
    /// </summary>
    void HideItemSprite()
    {
        if (_itemImage != null)
        {
            _itemImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Shows the spirit point cost and generates the UI objects necessary.
    /// </summary>
    void ShowSpiritPointCost()
    {
        if (_costParent != null && _costPrefab != null)
        {
            int cost = _markup.Properties["spirit_points"].IntegerValue;

            _costParent.gameObject.SetActive(true);

            // hide previously used instances
            foreach (GameObject costInstance in _costInstances)
            {
                costInstance.SetActive(false);
            }

            // instantiante more instances if necessary
            if (_costInstances.Count < cost)
            {
                for (int i = _costInstances.Count; i < cost; i++)
                {
                    GameObject instance = Instantiate(_costPrefab);
                    instance.transform.SetParent(_costParent);
                    _costInstances.Add(instance);
                }
            }

            // show appropriate instances
            for (int i = 0; i < cost; i++)
            {
                _costInstances[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Hides the spirit point cost
    /// </summary>
    void HideSpiritPointCost()
    {
        if (_costParent != null)
        {
            _costParent.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Shows the cancel button and provides it the DialogueOption
    /// </summary>
    /// <param name="option"></param>
    void ShowCancelButton(DialogueOption option)
    {
        _cancelButton.gameObject.SetActive(true);
        _cancelButton.Option = option;
        _cancelButton.OnOptionSelected = OptionViewWasSelected;
    }

    /// <summary>
    /// Hides the cancel button
    /// </summary>
    void HideCancelButton()
    {
        _cancelButton.gameObject.SetActive(false);
    }
}