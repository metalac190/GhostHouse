using UnityEngine;

namespace Animations
{
    [AddComponentMenu("Animators/AnimationBehavior")]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationEvents))]
    public class AnimationBehavior : MonoBehaviour, IInteractable
    {
        Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
            gameObject.layer = 9;
        }

        public void OnLeftClick(Vector3 mousePosition) {
        }

        public void OnRightClick(Vector3 mousePosition) {
        }

        public void OnLeftClick() {
            Debug.Log("Left Clicked");
            _animator.SetTrigger("Interact");
        }

        public void OnRightClick() {
        }

        public void OnHoverEnter() {
        }

        public void OnHoverExit() {
        }
    }
}