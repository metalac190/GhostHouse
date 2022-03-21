using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

namespace Mechanics.Animations
{
    [RequireComponent(typeof(Animator))]
    public class InteractionAnimation : MonoBehaviour
    {
        [Header("Animate Along")] [SerializeField]
        private bool _xAxis = true;

        [SerializeField] private bool _yAxis = false;
        [SerializeField] private bool _zAxis = false;

        private Animator _animator;

        private AnimatorController _controller;

        private AnimatorState _idleState;
        private AnimatorState _interactionState;
        private AnimatorState _postInteractionState;


        void Awake()
        {
            if (GetComponent<Animator>() != null) _animator = GetComponent<Animator>();
        }

        void Start()
        {
            CreateController();
        }

        private void CreateController()
        {
            string templatePath = "Assets/_Game/Entities/Interactables/AnimationControllers/_AC_Template.controller";
            string newPath = "Assets/_Game/Entities/Interactables/AnimationControllers/Temp/_AC_" + gameObject.name +
                             ".controller";

            AssetDatabase.CopyAsset(templatePath, newPath);
            _controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(newPath);

            AnimatorStateMachine rootSm = _controller.layers[0].stateMachine;
            ChildAnimatorState[] states = rootSm.states;

            _idleState = rootSm.defaultState;
            _idleState.motion = CreateIdleAnimation();

            _interactionState = states[1].state;
            _interactionState.motion = CreateInteractionAnimation();

            _postInteractionState = states[2].state;
            _postInteractionState.motion = CreateIdleAnimation();

            _controller.layers[0].stateMachine = rootSm;

            _animator.runtimeAnimatorController = _controller;
        }

        private AnimationClip CreateIdleAnimation()
        {
            AnimationClip idleAnimation = new AnimationClip();

            Keyframe[] keys = new Keyframe[2];
            keys[0] = new Keyframe(0f, 0f);
            keys[1] = new Keyframe(1f, 0f);
            AnimationCurve curve = new AnimationCurve(keys);
            idleAnimation.SetCurve("Art", typeof(Transform), "localPosition.x", curve);

            idleAnimation.name = "Idle";
            return idleAnimation;
        }

        private AnimationClip CreateInteractionAnimation()
        {
            AnimationClip interactionAnimation = new AnimationClip();

            if (_xAxis)
            {
                float startRot = gameObject.transform.rotation.x;
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, startRot);
                keys[1] = new Keyframe(0.25f, -25f);
                keys[2] = new Keyframe(0.75f, 25f);
                keys[3] = new Keyframe(1f, startRot);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.x", curve);
            }

            if (_yAxis)
            {
                float startRot = gameObject.transform.rotation.y;
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, startRot);
                keys[1] = new Keyframe(0.25f, -25f);
                keys[2] = new Keyframe(0.75f, 25f);
                keys[3] = new Keyframe(1f, startRot);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.y", curve);
            }

            if (_zAxis)
            {
                float startRot = gameObject.transform.rotation.z;
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, startRot);
                keys[1] = new Keyframe(0.25f, -25f);
                keys[2] = new Keyframe(0.75f, 25f);
                keys[3] = new Keyframe(1f, startRot);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.z", curve);
            }

            interactionAnimation.name = "Interaction";
            return interactionAnimation;
        }

        public void Interact()
        {
            _animator.SetTrigger("interact");
            Debug.Log("Did it");
        }

    }
}
