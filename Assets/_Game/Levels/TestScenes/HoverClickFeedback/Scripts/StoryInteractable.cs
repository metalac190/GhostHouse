using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels.TestScenes.HoverClickFeedback.Scripts
{
    public class StoryInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _overrideMaterialOnHover = false;
        [SerializeField] private Material _overrideMaterial = null;

        private List<MeshRenderer> _meshRenderers;
        private List<Material> _baseMaterial;

        private bool OverrideMaterial => _overrideMaterialOnHover && _overrideMaterial != null;

        public void OnLeftClick() {
        }

        public void OnRightClick() {
        }

        public void OnHoverEnter() {
            if (OverrideMaterial) {
                if (_meshRenderers == null) {
                    _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
                }
                _baseMaterial = new List<Material>();
                foreach (var meshRenderer in _meshRenderers) {
                    _baseMaterial.Add(meshRenderer.material);
                    meshRenderer.material = _overrideMaterial;
                }
            }
        }

        public void OnHoverExit() {
            if (OverrideMaterial) {
                for (var i = 0; i < _meshRenderers.Count; i++) {
                    _meshRenderers[i].material = _baseMaterial[i];
                }
                _baseMaterial.Clear();
            }
        }
    }
}