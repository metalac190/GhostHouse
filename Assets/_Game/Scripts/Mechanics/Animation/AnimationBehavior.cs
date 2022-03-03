using UnityEngine;

namespace Mechanics.Animations
{
    [AddComponentMenu("Animators/AnimationBehavior")]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationEvents))]
    public class AnimationBehavior : MonoBehaviour
    {
        Animator _anim;
        public Animator _animator => _anim;

        private void Awake() {
            _anim = GetComponent<Animator>();

        }
    }
}