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

    [Header("Choice")] 
    [SerializeField] private bool _override = false;


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
        if (AssetDatabase.LoadAssetAtPath<AnimationClip>(savePath) == null)
        {
            AssetDatabase.CreateAsset(clip, savePath);
        }
        else
        {
            if (_override) AssetDatabase.CreateAsset(clip,savePath);
        }
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

        var interaction = CreateInteractionAnimation();

        SaveAnimation(interaction);

        _idleState = rootSm.defaultState;

        _interactionState = states[1].state;
        _interactionState.motion = interaction;

        _postInteractionState = states[2].state;

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

        var geoObject = transform.GetChild(0).gameObject.transform.GetChild(0);
        var geoObjectName = "Art/" + geoObject.name;

        Keyframe[] keysXPos = new Keyframe[2];
        Keyframe[] keysYPos = new Keyframe[2];
        Keyframe[] keysZPos = new Keyframe[2];
        Keyframe[] keysXRot = new Keyframe[2];
        Keyframe[] keysYRot = new Keyframe[2];
        Keyframe[] keysZRot = new Keyframe[2];
        keysXPos[0] = new Keyframe(0f, geoObject.localPosition.x);
        keysXPos[1] = new Keyframe(1f, geoObject.localPosition.x);
        keysYPos[0] = new Keyframe(0f, geoObject.localPosition.y);
        keysYPos[1] = new Keyframe(1f, geoObject.localPosition.y);
        keysZPos[0] = new Keyframe(0f, geoObject.localPosition.z);
        keysZPos[1] = new Keyframe(1f, geoObject.localPosition.z);

        AnimationCurve curveX = new AnimationCurve(keysXPos);
        AnimationCurve curveY = new AnimationCurve(keysYPos);
        AnimationCurve curveZ = new AnimationCurve(keysZPos);

        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.x", curveX);
        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.y", curveY);
        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.z", curveZ);

        keysXRot[0] = new Keyframe(0f, geoObject.eulerAngles.x);
        keysXRot[1] = new Keyframe(1f, geoObject.eulerAngles.x);
        keysYRot[0] = new Keyframe(0f, geoObject.eulerAngles.y);
        keysYRot[1] = new Keyframe(1f, geoObject.eulerAngles.y);
        keysZRot[0] = new Keyframe(0f, geoObject.eulerAngles.z);
        keysZRot[1] = new Keyframe(1f, geoObject.eulerAngles.z);

        curveX = new AnimationCurve(keysXRot);
        curveY = new AnimationCurve(keysYRot);
        curveZ = new AnimationCurve(keysZRot);

        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localEulerAngles.x", curveX);
        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localEulerAngles.y", curveY);
        interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localEulerAngles.z", curveZ);

        if (_rotation)
        {
            if (_xAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysXRot[0].value);
                keys[1] = new Keyframe(0.25f, keysXRot[0].value - 25f);
                keys[2] = new Keyframe(0.75f, keysXRot[0].value + 25f);
                keys[3] = new Keyframe(1f, keysXRot[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.x", curve);
            }

            if (_yAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysYRot[0].value);
                keys[1] = new Keyframe(0.25f, keysYRot[0].value - 25f);
                keys[2] = new Keyframe(0.75f, keysYRot[0].value + 25f);
                keys[3] = new Keyframe(1f, keysYRot[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.y", curve);
            }

            if (_zAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysZRot[0].value);
                keys[1] = new Keyframe(0.25f, keysZRot[0].value - 25f);
                keys[2] = new Keyframe(0.75f, keysZRot[0].value + 25f);
                keys[3] = new Keyframe(1f, keysYRot[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve("Art", typeof(Transform), "localEulerAngle.z", curve);
            }
        }
        
        if (_position)
        {
            if (_xAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysXPos[0].value);
                keys[1] = new Keyframe(0.25f, keysXPos[0].value - 0.5f);
                keys[2] = new Keyframe(0.75f, keysXPos[0].value + 0.5f);
                keys[3] = new Keyframe(1f, keysXPos[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.x", curve);
            }

            if (_yAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysYPos[0].value);
                keys[1] = new Keyframe(0.25f, keysYPos[0].value - 0.5f);
                keys[2] = new Keyframe(0.75f, keysYPos[0].value + 0.5f);
                keys[3] = new Keyframe(1f, keysYPos[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.y", curve);
            }

            if (_zAxis)
            {
                Keyframe[] keys = new Keyframe[4];
                keys[0] = new Keyframe(0f, keysZPos[0].value);
                keys[1] = new Keyframe(0.25f, keysZPos[0].value - 0.5f);
                keys[2] = new Keyframe(0.75f, keysZPos[0].value + 0.5f);
                keys[3] = new Keyframe(1f, keysZPos[0].value);
                AnimationCurve curve = new AnimationCurve(keys);
                interactionAnimation.SetCurve(geoObjectName, typeof(Transform), "localPosition.z", curve);
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
