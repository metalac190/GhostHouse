using System.Collections.Generic;
using System.Linq;
using Mechanics.Feedback;
using Mechanics.UI;
using UnityEngine;
using Utility.Audio.Managers;

namespace Mechanics.Level_Mechanics
{
    public class StoryInteractable : InteractableBase
    {
        [Header("Seasons when Interactable")]
        [SerializeField] private Season _interactableSeasons = Season.Universal;

        [Header("On Hover")]
        [SerializeField] private bool _textOnHover = false;
        [SerializeField] private bool _hoverTextUseObjectName = false;
        [SerializeField] private string _hoverText = "";

        [SerializeField] private bool _highlightOnHover = false;
        [SerializeField] private Color _highlightColor = Color.yellow;
        [SerializeField] private float _highlightSize;

        [SerializeField] private bool _setMaterialOnHover = false;
        [SerializeField] private Material _materialToSet = null;

        [Header("On Click")]
        [SerializeField] private bool _sfxOnClick = false;
        [SerializeField] private SfxType _sfx = SfxType.Default;

        [Header("Interaction Window")]
        [SerializeField] private bool _popupWindowOnClick = false;
        [SerializeField, TextArea] private string _displayText = "";
        [SerializeField] private Sprite _imageToDisplay = null;
        [SerializeField] private bool _cancelButton = true;

        [Header("Interactions")]
        [SerializeField] private Interactable _interaction = null;
        [SerializeField] private string _interactionText = "Interact";
        [SerializeField] private Interactable _altInteraction = null;
        [SerializeField] private string _altInteractionText = "Alt Interact";

        //[Header("Collision Information")]
        //[SerializeField] private bool _confirmUseChildCollider = false;
        //[SerializeField] private bool _confirmUseSpecificCollider = false;
        //[SerializeField] private Collider _specificCollider = null;
        //private Collider colliderToUse = null;

        private List<MeshRenderer> _meshRenderers;
        private List<Material> _baseMaterial;

        private bool _missingHoverUi;

        #region Unity Functions

        private void Start() {
            if (TextHoverController.Singleton == null) {
                _missingHoverUi = true;
                Debug.LogWarning("Missing Text Hover Controller in Scene!");
            }
            if (_interaction != null) _interaction.LoadInteraction();
            if (_altInteraction != null) _altInteraction.LoadInteraction();
        }

        #endregion

        #region On Hover

        private bool TextOnHover => _textOnHover && (_hoverTextUseObjectName || !string.IsNullOrEmpty(_hoverText));
        private bool SetMaterialOnHover => _setMaterialOnHover && _materialToSet != null;

        public override void OnHoverEnter() {
            if (_missingHoverUi) return;
            if (TextOnHover) {
                string text = _hoverTextUseObjectName ? name : _hoverText;
                TextHoverController.Singleton.StartHover(text);
            }
            if (_highlightOnHover) {
                // TODO: Highlight
            }
            if (SetMaterialOnHover) {
                _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
                _baseMaterial = new List<Material>();
                foreach (var meshRenderer in _meshRenderers) {
                    _baseMaterial.Add(meshRenderer.material);
                    meshRenderer.material = _materialToSet;
                }
            }
        }

        public override void OnHoverExit() {
            if (_missingHoverUi) return;
            if (TextOnHover) {
                TextHoverController.Singleton.EndHover();
            }
            if (SetMaterialOnHover) {
                for (var i = 0; i < _meshRenderers.Count; i++) {
                    _meshRenderers[i].material = _baseMaterial[i];
                }
                _baseMaterial.Clear();
            }
        }

        #endregion

        #region On Click

        public override void OnLeftClick(Vector3 position) {
            Debug.Log("Clicked on " + gameObject.name + "! Interactable During " + _interactableSeasons);
            if (_sfxOnClick) {
                SoundManager.Instance.PlaySfx(_sfx, position);
            }
            if (_popupWindowOnClick) {
                ModalWindowController.Singleton.EnableModalWindow(_displayText, _imageToDisplay,
                    _cancelButton, _interaction, _interactionText,
                    _altInteraction, _altInteractionText);
            }
        }

        #endregion
    }
}