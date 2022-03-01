using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindowController : MonoBehaviour
{
    //private variables
    private Interactable _interactable;
    private Interactable _altInteractable;
    //private string _windowDisplayText;
    //private Sprite _windowDisplayImage;
    //private bool _cancelButton;

    //Actual Connections to Window
    [SerializeField] private GameObject _modalWindow = null;
    [SerializeField] private Button _mainInteractionButton = null;
    [SerializeField] private TextMeshProUGUI _mainInteractionText = null;
    [SerializeField] private Button _alternateInteractionButton = null;
    [SerializeField] private TextMeshProUGUI _alternateInteractionText = null;
    [SerializeField] private TextMeshProUGUI _modalWindowText = null;
    [SerializeField] private Button _closeButton = null;
    [SerializeField] private Image _displayImage = null;

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

        DisableModalWindow();
    }

    public void EnableModalWindow(string displayText, Sprite imageToDisplay, bool hasCancelButton,
        Interactable interactable, string interactButtonText, Interactable altInteractable, string altInteractButtonText) {
        // Enable Modal Window

        _modalWindowText.text = displayText;
        if (imageToDisplay != null) {
            _displayImage.gameObject.SetActive(true);
            _displayImage.sprite = imageToDisplay;
        }
        _closeButton.gameObject.SetActive(hasCancelButton);

        _interactable = null;
        if (interactable != null && interactable.CanInteract) {
            _interactable = interactable;
            _mainInteractionButton.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(interactButtonText)) {
                _mainInteractionText.text = interactButtonText;
            }
        }

        _altInteractable = null;
        if (altInteractable != null && altInteractable.CanInteract) {
            _altInteractable = altInteractable;
            _alternateInteractionButton.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(altInteractButtonText)) {
                _alternateInteractionText.text = altInteractButtonText;
            }
        }

        _modalWindow.SetActive(true);
    }

    public void ClickButton() {
        DisableModalWindow();
        if (_interactable != null) _interactable.Interact();
    }

    public void ClickAltButton() {
        DisableModalWindow();
        if (_altInteractable != null) _altInteractable.Interact();
    }

    public void DisableModalWindow() {
        _modalWindowText.text = "";
        _displayImage.gameObject.SetActive(false);
        _closeButton.gameObject.SetActive(true);
        _mainInteractionButton.gameObject.SetActive(false);
        _mainInteractionText.text = "Interact";
        _alternateInteractionButton.gameObject.SetActive(false);
        _alternateInteractionText.text = "Interact";
        _modalWindow.SetActive(false);
    }

    #region Debug Methods

    public void MainWorked() {
        Debug.Log("Main Interaction Is Done");
    }

    #endregion
}