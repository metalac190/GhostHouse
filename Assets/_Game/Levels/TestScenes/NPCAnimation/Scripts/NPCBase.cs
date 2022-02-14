using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class NPCBase : MonoBehaviour, IInteractable
    {
        [Header("NPC Details")]
        [SerializeField] protected string _name = string.Empty;
        [SerializeField] protected int _age = 0;
        [Tooltip("i.e. younger sister or grandmother")]
        [SerializeField] protected string _placeInFamily = string.Empty;

        [Header("Animations")]
        [SerializeField] protected AnimationClip _angryAnimation = null;
        [SerializeField] protected AnimationClip _happyAnimation = null;
        [SerializeField] protected AnimationClip _idleAnimation = null;
        [SerializeField] protected AnimationClip _sadAnimation = null;
        [SerializeField] protected AnimationClip _surprisedAnimation = null;

        [Tooltip("Can be used to insure where the NPC goes in between playthroughs and levels")]
        [Header("Preset Locations")]
        [SerializeField] protected Transform _levelPosition = null;

        [Header("Sounds")]
        [SerializeField] protected AudioClip _sound1 = null;

        Rigidbody _rb;
        Collider _collider;
        AudioSource _audioSource;
        Animator _animator;

        public AnimationClip IdleAnimation => _idleAnimation;
        public AnimationClip SurprisedAnimation => _surprisedAnimation;
        public AnimationClip HappyAnimation => _happyAnimation;
        public AnimationClip SadAnimation => _sadAnimation;
        public AnimationClip AngryAnimation => _angryAnimation;

        public abstract void Idle();
        public abstract void Talking();
        public abstract void Reactions();
        public abstract void Dialogue();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        #region Interaction

        //This is when the mouse first hovers over the object.
        public void OnHoverEnter()
        {
            //Debug.Log("Hovering over " + gameObject.name);
            _animator.SetTrigger("TriggerAngry");
        }

        //This is when the mouse leaves the shape of the object.
        public void OnHoverExit()
        {
            //Debug.Log("No Longer Hovering over" + gameObject.name);
            _animator.SetTrigger("TriggerSad");
        }

        //This is when the mouse left clicks on the object.
        public void OnLeftClick()
        {
            Debug.Log("Left Clicked On" + gameObject.name);
            _animator.SetTrigger("TriggerHappy");
        }

        //This is when the mouse right clicks on an object.
        public void OnRightClick()
        {
            Debug.Log("Right Clicked On" + gameObject.name);
            _animator.SetTrigger("TriggerSurprised");
        }
        #endregion
    }
}
