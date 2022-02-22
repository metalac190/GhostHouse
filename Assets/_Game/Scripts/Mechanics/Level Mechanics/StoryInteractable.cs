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
        [Header("Sfx On Click")]
        [SerializeField] private bool _sfxOnClick = false;
        [SerializeField] private SfxType _sfx = SfxType.None;

        [Header("Text On Hover")]
        [SerializeField] private bool _textOnHover = false;
        [SerializeField] private bool _useObjectName = false;
        [SerializeField] private string _hoverText = "";

        [Header("Material Override On Hover")]
        [SerializeField] private bool _setMaterialOnHover = false;
        [SerializeField] private Material _materialToSet = null;

        private List<MeshRenderer> _meshRenderers;
        private List<Material> _baseMaterial;

        private bool _missingHoverUi;

        private bool OverrideText => _textOnHover && (_useObjectName || !string.IsNullOrEmpty(_hoverText));
        private bool OverrideMaterial => _setMaterialOnHover && _materialToSet != null;

        private void Start() {
            if (TextHoverController.Singleton == null) {
                _missingHoverUi = true;
                Debug.LogWarning("Missing Text Hover Controller in Scene!");
            }
        }

        public void OnLeftClick() {
        }

        public void OnLeftClick(Vector3 position) {
            if (_sfxOnClick) {
                SoundManager.Instance.PlaySfx(_sfx, position);
            }
        }

        public void OnRightClick() {
        }

        public void OnRightClick(Vector3 position) {
            if (_sfxOnClick) {
                SoundManager.Instance.PlaySfx(_sfx, position);
            }
        }


        public void OnHoverEnter() {
            if (_missingHoverUi) return;
            if (OverrideText) {
                string text = _useObjectName ? name : _hoverText;
                TextHoverController.Singleton.StartHover(text);
            }
            if (OverrideMaterial) {
                if (_meshRenderers == null) {
                    _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
                }
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