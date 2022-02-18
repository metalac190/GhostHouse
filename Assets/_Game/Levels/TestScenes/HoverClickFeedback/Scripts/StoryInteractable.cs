using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Audio.Clips.Base;

namespace Levels.TestScenes.HoverClickFeedback.Scripts
{
    public class StoryInteractable : MonoBehaviour, IInteractable
    {
        [Header("Sfx On Click")]
        [SerializeField] private bool _sfxOnClick = false;
        [SerializeField] private SfxBase _sfx = null;

        [Header("Text On Hover")]
        [SerializeField] private bool _textOnHover = false;
        [SerializeField] private bool _useObjectName = false;
        [SerializeField] private string _hoverText = "";

        [Header("Material Override On Hover")]
        [SerializeField] private bool _setMaterialOnHover = false;
        [SerializeField] private Material _materialToSet = null;

        private List<MeshRenderer> _meshRenderers;
        private List<Material> _baseMaterial;

        private bool OverrideText => _textOnHover && (_useObjectName || !string.IsNullOrEmpty(_hoverText));
        private bool OverrideMaterial => _setMaterialOnHover && _materialToSet != null;

        public void OnLeftClick() {
            if (_sfxOnClick) {
                _sfx.Play();
            }
        }

        public void OnRightClick() {
            if (_sfxOnClick) {
                _sfx.Play();
            }
        }

        public void OnHoverEnter() {
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

        public void OnHoverExit() {
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