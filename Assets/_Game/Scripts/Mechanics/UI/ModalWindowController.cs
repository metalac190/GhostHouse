using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UI;

public class ModalWindowController : MonoBehaviour
{
    //private variables
    //private Interactable _interactable;
    //private Interactable _altInteractable;
    //private string _windowDisplayText;
    //private Sprite _windowDisplayImage;
    //private bool _cancelButton;

    //Actual Connections to Window
    [SerializeField] private GameObject _modalWindow = null;
    [SerializeField] private Button _mainInteractionButton = null;
    [SerializeField] private TextMeshProUGUI _mainInteractionText = null;
    [SerializeField] private List<Image> _spiritPoints = new List<Image>();
    [SerializeField] private List<Image> _altSpiritPoints = new List<Image>();
    [SerializeField] private Sprite _spiritPointSpend = null;
    [SerializeField] private Sprite _spiritPointCannotSpend = null;
    [SerializeField] private Button _alternateInteractionButton = null;
    [SerializeField] private TextMeshProUGUI _alternateInteractionText = null;
    [SerializeField] private TextMeshProUGUI _closeButton = null;

    [Header("HUD")]
    [SerializeField] private PlayerHUD _playerHud = null;

    public static event Action OnInteractStart = delegate { };
    public static event Action OnInteractEnd = delegate { };

    private bool _enabled;

    #region Singleton Pattern

    public static ModalWindowController Singleton { get; private set; }

    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        #endregion
    }

    private void Start() {
        DisableModalWindow();
    }

    private void Update() {
        if (_enabled && Input.GetKeyDown(KeyCode.Escape)) {
            DisableModalWindow();
        }
    }

    public void EnableModalWindow(string closeButtonText, Action callback, string interactButtonText, Action altCallback, string altInteractButtonText, int pointsToSpend, int altPointsToSpend) {
        int maxPointsToSpend = 0;
        int currentPoints = DataManager.Instance.remainingSpiritPoints;

        bool canSpendPoints = pointsToSpend == 0 || pointsToSpend <= currentPoints;
        if (canSpendPoints) {
            maxPointsToSpend = pointsToSpend;
        }
        for (var i = 0; i < _spiritPoints.Count; i++) {
            _spiritPoints[i].sprite = canSpendPoints ? _spiritPointSpend : _spiritPointCannotSpend;
            _spiritPoints[i].transform.parent.gameObject.SetActive(i < pointsToSpend);
        }

        bool canSpendAltPoints = pointsToSpend == 0 || pointsToSpend <= currentPoints;
        if (canSpendAltPoints && altPointsToSpend > maxPointsToSpend) {
            maxPointsToSpend = altPointsToSpend;
        }
        for (var i = 0; i < _spiritPoints.Count; i++) {
            _spiritPoints[i].sprite = canSpendAltPoints ? _spiritPointSpend : _spiritPointCannotSpend;
            _altSpiritPoints[i].transform.parent.gameObject.SetActive(i < pointsToSpend);
        }

        if (_playerHud != null) _playerHud.UpdateSpiritPoints(maxPointsToSpend);

        // Enable Modal Window
        IsometricCameraController.Singleton._interacting = true;
        OnInteractStart?.Invoke();

        if (callback != null) {
            _mainInteractionButton.gameObject.SetActive(true);
            _mainInteractionButton.interactable = canSpendPoints;
            if (canSpendPoints) {
                _mainInteractionButton.onClick.AddListener(callback.Invoke);
                _mainInteractionButton.onClick.AddListener(DisableModalWindow);
            }
            if (!string.IsNullOrEmpty(interactButtonText)) {
                _mainInteractionText.text = interactButtonText;
            }
        }

        _closeButton.text = closeButtonText;

        if (altCallback != null) {
            _alternateInteractionButton.gameObject.SetActive(true);
            _alternateInteractionButton.interactable = canSpendAltPoints;
            if (canSpendAltPoints) {
                _alternateInteractionButton.onClick.AddListener(altCallback.Invoke);
                _alternateInteractionButton.onClick.AddListener(DisableModalWindow);
            }
            if (!string.IsNullOrEmpty(altInteractButtonText)) {
                _alternateInteractionText.text = altInteractButtonText;
            }
        }

        _modalWindow.SetActive(true);
        _enabled = true;
        PauseMenu.Singleton.PreventPausing(false);
    }

    public void DisableModalWindow() {
        if (_playerHud != null) _playerHud.UpdateSpiritPoints();
        OnInteractEnd?.Invoke();
        _mainInteractionButton.gameObject.SetActive(false);
        _mainInteractionButton.onClick.RemoveAllListeners();
        _mainInteractionText.text = "Interact";
        _alternateInteractionButton.gameObject.SetActive(false);
        _alternateInteractionButton.onClick.RemoveAllListeners();
        _alternateInteractionText.text = "Interact";
        _modalWindow.SetActive(false);
        _enabled = false;
        if (PauseMenu.Singleton != null) {
            PauseMenu.Singleton.PreventPausing(true);
        }
        if (IsometricCameraController.Singleton != null) {
            IsometricCameraController.Singleton._interacting = false;
        }
    }

    #region Debug Methods

    public void MainWorked() {
        Debug.Log("Main Interaction Is Done");
    }

    #endregion
}