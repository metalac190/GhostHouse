using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Level_Mechanics
{
    public class InteractableMeshConnection : MonoBehaviour
    {
        [SerializeField] private Interactable _interactable = null;
        [SerializeField] private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
        [SerializeField] private List<Animator> _animators = new List<Animator>();

        private void OnEnable() {
            if (_interactable == null) return;
            foreach (var meshRenderer in _meshRenderers) {
                _interactable.ConnectedMeshRenderers.Add(meshRenderer);
            }
            foreach (var animators in _animators) {
                _interactable.ConnectedAnimators.Add(animators);
            }
        }

        private void OnDisable() {
            if (_interactable == null) return;
            foreach (var meshRenderer in _meshRenderers) {
                _interactable.ConnectedMeshRenderers.Remove(meshRenderer);
            }
            foreach (var animators in _animators) {
                _interactable.ConnectedAnimators.Remove(animators);
            }
        }
    }
}