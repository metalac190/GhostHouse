#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using Utility.Buttons;

[RequireComponent(typeof(Animator))]
public class SquashStretchAnimation : MonoBehaviour
{
    [Header("Stretch Axis")]
    [SerializeField] private bool _xAxis = false;
    [SerializeField] private bool _zAxis = false;

    [Header("Animation Values")] 
    [SerializeField] private float _squashAmount = 1f;
    [SerializeField] private float _stretchAmount = 1f;

    public float StretchAmount => _stretchAmount;

    private Animator _animator;

    private AnimatorController _controller;

    private AnimatorState _idleState;
    private AnimatorState _interactionState;
    private AnimatorState _postInteractionState;
    private AnimatorState _onClickState;

    private void Awake()
    {
        if (GetComponent<Animator>() != null) _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        var controller = (AnimatorController)AssetDatabase.LoadAssetAtPath("Assets/_Game/Entities/Interactables/Temp/Controllers/_AC_" + gameObject.name +
                                      ".controller", typeof(AnimatorController));

        //_animator.runtimeAnimatorController = controller;
    }

    private void SaveAnimation(AnimationClip clip)
    {
        string savePath = "Assets/_Game/Entities/Interactables/Temp/Animations/anim_" + gameObject.name + "_" +
                          clip.name + ".anim";
        AssetDatabase.CreateAsset(clip, savePath);
    }

    private void AssignController()
    {
        var controller = (AnimatorController)AssetDatabase.LoadAssetAtPath("Assets/_Game/Entities/Interactables/Temp/Controllers/_AC_" + gameObject.name +
                                                                           ".controller", typeof(AnimatorController));
        _animator.runtimeAnimatorController = controller;
    }

    [Button(Mode = ButtonMode.WhilePlaying)]
    private void CreateControllerAndAnimation()
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
        var anim = CreateAnimation();

        //SaveAnimation(idle);
        SaveAnimation(anim);

        _idleState = rootSm.defaultState;
        //_idleState.motion = idle;

        _interactionState = states[1].state;
        //_interactionState.motion = anim;

        _postInteractionState = states[2].state;
        //_postInteractionState.motion = idle;

        _onClickState = states[3].state;
        _onClickState.motion = anim;

        _controller.layers[0].stateMachine = rootSm;

        AssignController();
    }

    [Button(Mode = ButtonMode.WhilePlaying)]
    private void ClearController()
    {
        AnimatorStateMachine rootSm = _controller.layers[0].stateMachine;
        ChildAnimatorState[] states = rootSm.states;

        //var idle = CreateIdleAnimation();
        var anim = CreateAnimation();

        //SaveAnimation(idle);
        SaveAnimation(anim);

        _idleState = rootSm.defaultState;

        _interactionState = states[1].state;
        _interactionState.motion = null;

        _postInteractionState = states[2].state;
        _postInteractionState.motion = null;

        _onClickState = states[3].state;
        _onClickState.motion = anim;

        _controller.layers[0].stateMachine = rootSm;
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

    private AnimationClip CreateAnimation()
    {
        if (_squashAmount <= 0f)
        {
            Debug.LogError("Squash scale cannot be less than 0");
            return null;
        }

        AnimationClip anim = new AnimationClip();

        float startYScale = gameObject.transform.localScale.y;

        Keyframe[] squashKeys = new Keyframe[3];
        squashKeys[0] = new Keyframe(0f, startYScale);
        squashKeys[1] = new Keyframe(0.5f, _squashAmount);
        squashKeys[2] = new Keyframe(1f, startYScale);
        AnimationCurve squashCurve = new AnimationCurve(squashKeys);

        Keyframe[] stretchKeys = new Keyframe[3];
        stretchKeys[0] = new Keyframe(0f, startYScale);
        stretchKeys[1] = new Keyframe(0.5f, _squashAmount);
        stretchKeys[2] = new Keyframe(1f, startYScale);
        AnimationCurve stretchCurve = new AnimationCurve(stretchKeys);

        anim.SetCurve("Art", typeof(Transform), "localScale.y", squashCurve);

        if (_xAxis) anim.SetCurve("Art", typeof(Transform), "localScale.x", stretchCurve);
        if (_zAxis) anim.SetCurve("Art", typeof(Transform), "localScale.z", stretchCurve);

        anim.name = "SquashAndStretch";
        return anim;
    }

    public void Interact()
    {
        _animator.SetTrigger("interact");
    }

}
#endif
