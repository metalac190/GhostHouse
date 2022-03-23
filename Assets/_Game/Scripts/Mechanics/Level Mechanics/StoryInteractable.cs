using System;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Feedback;
using Mechanics.UI;
using UnityEngine;
using UnityEngine.Events;
using Utility.Audio.Managers;

namespace Mechanics.Level_Mechanics
{
    public class StoryInteractable : InteractableBase
    {
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
        [SerializeField] private bool _moveOnClick = true;
        [SerializeField] private float _cameraMovementTime = 3f;

        [Header("Interaction Window")]
        [SerializeField] private bool _popupWindowOnClick = false;
        [SerializeField] private Interactable _interaction = null;
        [SerializeField] private string _interactionText = "Interact";
        [SerializeField] private Interactable _alternateInteraction = null;
        [SerializeField] private string _alternateInteractionText = "Alt Interact";
        [SerializeField] private string _closeMenuText = "Close";

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
            if (_interaction != null) {
                DataManager.Instance.SetDefaultInteraction(_interaction.name);
            }
            if (_alternateInteraction != null) {
                DataManager.Instance.SetDefaultInteraction(_alternateInteraction.name);
            }
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

        public override void OnLeftClick(Vector3 mousePosition) {
            if (_sfxOnClick) {
                SoundManager.Instance.PlaySfx(_sfx, mousePosition);
            }
            if (_popupWindowOnClick && !(IsometricCameraController.Singleton._dragging)) {
                Action callback = _interaction != null && _interaction.CanInteract ? (Action)Interact : null;
                Action altCallback = (_alternateInteraction != null && _alternateInteraction.CanInteract) ? (Action)AltInteract : null;
                int maxPointsSpent = Mathf.Max(_interaction != null ? _interaction.Cost : 0, _alternateInteraction != null ? _alternateInteraction.Cost : 0);

                ModalWindowController.Singleton.EnableModalWindow(_closeMenuText, callback, _interactionText, altCallback, _alternateInteractionText, maxPointsSpent);
            }
            else if (!_popupWindowOnClick && _interaction != null) {
                _interaction.Interact();
            }
            if (_moveOnClick) {
                IsometricCameraController.Singleton.MoveToPosition(mousePosition, _cameraMovementTime);
            }

            //if (_moveOnClick)
            //{
            //    StartCoroutine(IsometricCameraController.Singleton.MoveToPosition(transform.position, _cameraMovementTime));
            //}
        }

        public void Interact() {
            _interaction.Interact();
        }

        public void AltInteract() {
            _alternateInteraction.Interact();
        }

        #endregion
    }
}