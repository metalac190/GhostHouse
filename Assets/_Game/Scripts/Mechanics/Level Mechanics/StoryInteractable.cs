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
        [Header("Hover: Preview Text")]
        [SerializeField] private bool _previewText = false;
        [SerializeField] private bool _useObjectName = false;
        [SerializeField] private string _hoverText = "";

        [Header("Hover: Highlight")]
        [SerializeField] private bool _highlight = false;
        [SerializeField] private Color _highlightColor = Color.yellow;
        [SerializeField] private float _highlightSize = 1f;

        [Header("Hover: Override Art Materials")]
        [SerializeField] private bool _clickOverrideMaterial = false;
        [SerializeField] private Material _materialToSet = null;

        [Header("Click: Sfx")]
        [SerializeField] private bool _clickSfx = false;
        [SerializeField] private SfxType _sfx = SfxType.None;

        [Header("Click: Modal Window")]
        [SerializeField] private bool _clickWindow = false;
        [SerializeField, TextArea] private string _displayText = "";
        [SerializeField] private bool _displayImage = false;
        [SerializeField] private Sprite _imageToDisplay = null;
        [SerializeField] private bool _cancelButton = true;

        [Header("Interaction")]
        [SerializeField] private bool _confirmButton = true;
        [SerializeField] private Interactable _interaction = null;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Animator _confirmAnimation = null;

        [Header("Alternate Interaction")]
        [SerializeField] private bool _confirmAltButton = false;
        [SerializeField] private Interactable _altInteraction = null;
        [SerializeField] private Sprite _altSprite;
        [SerializeField] private Animator _confirmAltAnimation = null;

        [Header("Collision Information")]
        [SerializeField] private bool _confirmUseChildCollider = false;
        [SerializeField] private bool _confirmUseSpecificCollider = false;
        [SerializeField] private Collider _specificCollider = null;
        private Collider childCollider = null;


        private List<MeshRenderer> _meshRenderers;
        private List<Material> _baseMaterial;

        private bool _missingHoverUi;

        private bool OverrideText => _previewText && (_useObjectName || !string.IsNullOrEmpty(_hoverText));
        private bool OverrideMaterial => _clickOverrideMaterial && _materialToSet != null;

        private void Start() {
            if (TextHoverController.Singleton == null) {
                _missingHoverUi = true;
                Debug.LogWarning("Missing Text Hover Controller in Scene!");
            }
        }

        public override void OnLeftClick(Vector3 position) {
            if (_clickSfx) {
                SoundManager.Instance.PlaySfx(_sfx, position);
            }
            PanelController.Singleton.EnableModalWindow(_interaction, _altInteraction);
        }

        public override void OnRightClick(Vector3 position) {
            if (_clickSfx) {
                SoundManager.Instance.PlaySfx(_sfx, position);
            }
        }


        public override void OnHoverEnter() {
            if (_missingHoverUi) return;
            if (OverrideText) {
                string text = _useObjectName ? name : _hoverText;
                TextHoverController.Singleton.StartHover(text);
            }
            if (OverrideMaterial) {
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
            if (OverrideText) {
                TextHoverController.Singleton.EndHover();
            }
            if (OverrideMaterial) {
                for (var i = 0; i < _meshRenderers.Count; i++) {
                    _meshRenderers[i].material = _baseMaterial[i];
                }
                _baseMaterial.Clear();
            }
        }
    }
}