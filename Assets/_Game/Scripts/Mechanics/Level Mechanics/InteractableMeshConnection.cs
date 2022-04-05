using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Level_Mechanics
{
    public class InteractableMeshConnection : MonoBehaviour
    {
        [SerializeField] private Interactable _interactable = null;
        [SerializeField] private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();

        private void OnEnable() {
            if (_interactable == null) return;
            foreach (var meshRenderer in _meshRenderers) {
                _interactable.ConnectedMeshRenderers.Add(meshRenderer);
            }
        }

        private void OnDisable() {
            if (_interactable == null) return;
            foreach (var meshRenderer in _meshRenderers) {
                _interactable.ConnectedMeshRenderers.Remove(meshRenderer);
            }
        }
    }
}