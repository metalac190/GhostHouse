using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Mechanics.Feedback;
using UI;
using Utility.Audio.Helper;

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
    [SerializeField] private GameObject _raycastBlock = null;
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

    [Header("Sfx")]
    [SerializeField] private SfxReference _openWindow = new SfxReference(true);
    [SerializeField] private SfxReference _cancelOrCloseWindow = new SfxReference(true);
    [SerializeField] private SfxReference _journalNotification = new SfxReference(true);
    [SerializeField] private SfxReference _spendSpiritPoint = new SfxReference(true);
    [SerializeField] private SfxReference _spendAllSpiritPoints = new SfxReference(true);


    public static event Action OnInteractStart = delegate { };
    public static event Action OnInteractEnd = delegate { };
    private Action _callback;

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
        DisableModalWindow(false, false);
    }

    private void Update() {
        if (_enabled) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                DisableModalWindow();
            } else if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && _callback != null) {
                _callback.Invoke();
                _callback = null;
                DisableModalWindow();
            }
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

        _callback = callback;
        if (callback != null) {
            _mainInteractionButton.gameObject.SetActive(true);
            _mainInteractionButton.interactable = canSpendPoints;
            if (canSpendPoints) {
                _mainInteractionButton.onClick.AddListener(callback.Invoke);
                _mainInteractionButton.onClick.AddListener(InteractionCloseWindow);
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
                _alternateInteractionButton.onClick.AddListener(InteractionCloseWindow);
            }
            if (!string.IsNullOrEmpty(altInteractButtonText)) {
                _alternateInteractionText.text = altInteractButtonText;
            }
        }

        _modalWindow.SetActive(true);
        if (_raycastBlock != null) _raycastBlock.SetActive(true);
        _enabled = true;
        PauseMenu.Singleton.PreventPausing(false);
        _openWindow.Play();
    }

    public void InteractionCloseWindow() {
        DisableModalWindow(false);
    }

    public void DisableModalWindow() => DisableModalWindow(true);
    public void DisableModalWindow(bool playSound, bool updateCanPause = true) {
        if (_playerHud != null) _playerHud.UpdateSpiritPoints();
        OnInteractEnd?.Invoke();
        _mainInteractionButton.onClick.RemoveAllListeners();
        _mainInteractionButton.gameObject.SetActive(false);
        _mainInteractionText.text = "Interact";
        _alternateInteractionButton.onClick.RemoveAllListeners();
        _alternateInteractionButton.gameObject.SetActive(false);
        _alternateInteractionText.text = "Interact";
        _modalWindow.SetActive(false);
        if (_raycastBlock != null) _raycastBlock.SetActive(false);
        _enabled = false;
        if (updateCanPause) StartCoroutine(CanPauseNextFrame());
        if (IsometricCameraController.Singleton != null) {
            IsometricCameraController.Singleton._interacting = false;
        }
        if (playSound) _cancelOrCloseWindow.Play();
    }

    public IEnumerator CanPauseNextFrame() {
        yield return null;
        if (PauseMenu.Singleton != null) {
            PauseMenu.Singleton.PreventPausing(true);
        }
    }

    public void AddJournalNotification() {
        if (_playerHud == null) return;
        _playerHud.AddJournalNotification();
        _journalNotification.Play();
    }

    public void HideHudOnPause(bool pause) {
        if (_playerHud == null) return;
        if (pause) _playerHud.ClearJournalNotification();
        _playerHud.gameObject.SetActive(!pause);
    }

    public void PlaySpiritPointSpentSounds(bool usedAll) {
        if (usedAll) {
            _spendAllSpiritPoints.Play();
        }
        else {
            _spendSpiritPoint.Play();
        }
    }

    #region Debug Methods

    public void MainWorked() {
        Debug.Log("Main Interaction Is Done");
    }

    public void ForceUpdateHudSpiritPoints() {
        if (!_enabled) _playerHud.UpdateSpiritPoints();
    }

    #endregion
}