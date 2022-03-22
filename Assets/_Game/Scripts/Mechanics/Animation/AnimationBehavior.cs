using UnityEngine;

namespace Mechanics.Animations
{
    [AddComponentMenu("Animators/AnimationBehavior")]
    [RequireComponent(typeof(Animator))]
    public class AnimationBehavior : MonoBehaviour
    {
        Animator _anim;
        public Animator _animator => _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void Interacted()
        {
            _anim.SetTrigger("interact");
        }
    }
}