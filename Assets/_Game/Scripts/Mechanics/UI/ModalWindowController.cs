using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindowController : MonoBehaviour
{
    //private variables
    private Interactable _mainInteraction;
    private Interactable _alternateInteraction;
    //private string _windowDisplayText;
    //private Sprite _windowDisplayImage;
    //private bool _cancelButton;

    //Actual Connections to Window
    [SerializeField] GameObject _modalWindow;
    [SerializeField] Button _mainInteractionButton = null;
    [SerializeField] Button _alternateInteractionButton = null;
    [SerializeField] TextMeshProUGUI _modalWindowText = null;
    [SerializeField] Button _closeButton = null;
    [SerializeField] Image _displayImage = null;

    private Interactable _interactable;
    private Interactable _altInteractable;

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

        _modalWindow.SetActive(false);
    }


    public void EnableModalWindow(Interactable interactable, Interactable altInteractable, string displayText, Sprite imageToDisplay, bool hasCancelButton,
        string mainIntText, string altIntText) {
        // Carlos here

        _interactable = interactable;
        _altInteractable = interactable;

        #region Assigning Variables

        if (interactable == null) {
            Debug.LogWarning("There is no InteractableObject created/connected!");
        }
        else {
            _mainInteraction = interactable;
        }

        if (altInteractable == null) {
            //Note to Carlos and UI Designers: If altInteractable is null, then you only need one button for the Modal Window
            //Note to Carlos and UI Designers: feel free to disable the second button here.
            _alternateInteractionButton.gameObject.SetActive(false);
        }
        else {
            _alternateInteraction = altInteractable;
        }

        if (displayText.Equals("")) {
            _modalWindowText.text = "Default Text";
        }
        else {
            _modalWindowText.text = displayText;
        }

        if (imageToDisplay == null) {
            //Note to Carlos and UI Designers: If imageToDisplay is null, then you can disable the sprite for the Modal Window
            //Note to Carlos and UI Designers: feel free to disable the sprite here.
        }
        else {
            _displayImage.sprite = imageToDisplay;
        }

        #endregion

        #region Displaying The Window

        if (_modalWindow.activeInHierarchy == false) {
            _modalWindow.SetActive(true);
        }

        if (hasCancelButton) {
            _closeButton.gameObject.SetActive(true);
        }
        else {
            _closeButton.gameObject.SetActive(false);
        }

        #endregion
    }

    public void ClickButton() {
        _interactable.Interact();
        DisableModalWindow();
    }

    public void ClickAltButton() {
        _altInteractable.Interact();
        DisableModalWindow();
    }

    public void DisableModalWindow() {
        //disable the window here
        _modalWindow.SetActive(false);
    }

    #region Debug Methods

    public void MainWorked() {
        Debug.Log("Main Interaction Is Done");
    }

    #endregion
}