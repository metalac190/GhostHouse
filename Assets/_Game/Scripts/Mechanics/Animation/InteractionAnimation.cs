#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor.VersionControl;
using Utility.Buttons;

[RequireComponent(typeof(Animator))]
public class InteractionAnimation : MonoBehaviour
{
    [Header("Transform Type")] 
    [SerializeField] private bool _position = false;
    [SerializeField] private bool _rotation = true;

    [Header("Animate Along")]
    [SerializeField] private bool _xAxis = true;
    [SerializeField] private bool _yAxis = false;
    [SerializeField] private bool _zAxis = false;

    private Animator _animator;

    private AnimatorController _controller;

    private AnimatorState _idleState;
    private AnimatorState _interactionState;
    private AnimatorState _postInteractionState;

    private void Awake()
    {
        if (GetComponent<Animator>() != null) _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        var controller = (AnimatorController) AssetDatabase.LoadAssetAtPath("Assets/_Game/Entities/Interactables/Temp/Controllers/_AC_" + gameObject.name +
                                      ".controller",typeof(AnimatorController));

        //_animator.runtimeAnimatorController = controller;
    }

    private void SaveAnimation(AnimationClip clip)
    {
        string savePath = "Assets/_Game/Entities/Interactables/Temp/Animations/anim_" + gameObject.name + "_" +
                          clip.name + ".anim";
        AssetDatabase.CreateAsset(clip,savePath);
    }

    private void AssignController()
    {
        var controller = (AnimatorController)AssetDatabase.LoadAssetAtPath("Assets/_Game/Entities/Interactables/Temp/Controllers/_AC_" + gameObject.name +
                                                                           ".controller", typeof(AnimatorController));
        _animator.runtimeAnimatorController = controller;
    }

    [Button(Mode = ButtonMode.WhilePlaying)]
    private void CreateControllerAndAnimations()
    {
        if (GetComponent<Animator>() != null) _animator = GetComponent<Animator>();

        string templatePath = "Assets/_Game/Entities/Interactables/AnimationControllers/_AC_Template.controller";
        string newPath = "Assets/_Game/Entities/Interactables/Temp/Controllers/_AC_" + gameObject.name +
                         ".controller";

        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(newPath) == null)
        {
            AssetDatabase.CopyAsset(templatePath, newPath);
        }
        _controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(newPath);

        AnimatorStateMachine rootSm = _controller.layers[0].stateMachine;
        ChildAnimatorState[] states = rootSm.states;

        //var idle = CreateIdleAnimation();
        var interaction = CreateInteractionAnimation();

        //SaveAnimation(idle);
        SaveAnimation(interaction);

        _idleState = rootSm.defaultState;
        //_idleState.motion = idle;

        _interactionState = states[1].state;
        _interactionState.motion = interaction;

        _postInteractionState = states[2].state;
        //_postInteractionState.motion = idle;

        _controller.layers[0].stateMachine = rootSm;

        AssignController();
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

        if (_rotation)
        {
            if (_xAxis)
            {
                float startRot = gameObject.transform.localRotation.x;
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
                float startRot = gameObject.transform.localRotation.y;
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
                float startRot = gameObject.transform.localRotation.z;
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, startRot);
                keys[1] = new Keyframe(0.25f, -25f);
                keys[2] = new Keyframe(0.75f, 25f);
                keys[3] = new Keyframe(1f, startRot);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.z", curve);
            }
        }
        else if (_position)
        {
            if (_xAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, 0);
                keys[1] = new Keyframe(0.25f, -0.5f);
                keys[2] = new Keyframe(0.75f, 0.5f);
                keys[3] = new Keyframe(1f, 0);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localPosition.x", curve);
            }

            if (_yAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, 0);
                keys[1] = new Keyframe(0.25f, -0.5f);
                keys[2] = new Keyframe(0.75f, 0.5f);
                keys[3] = new Keyframe(1f, 0);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localPosition.y", curve);
            }

            if (_zAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, 0);
                keys[1] = new Keyframe(0.25f, -0.5f);
                keys[2] = new Keyframe(0.75f, 0.5f);
                keys[3] = new Keyframe(1f, 0);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localPosition.z", curve);
            }
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
#endif
