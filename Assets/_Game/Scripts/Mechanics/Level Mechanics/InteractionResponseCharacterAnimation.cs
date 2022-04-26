using UnityEngine;

public class InteractionResponseCharacterAnimation : InteractableResponseBase
{
    [SerializeField] private Animator _animator;

    [Header("Animation To Play (only Val and Jaq have angry animations)")]
    [SerializeField] private bool _surpriseAnimation = false;
    [Tooltip("Only Val and Jaq have angry animations")]
    [SerializeField] private bool _angryAnimation = false;

    [Header("For Morgan Only")] 
    [SerializeField] private bool _sleepAnimation = false;

    [Header("For Val Only")] 
    [SerializeField] private bool _pianoAnimation = false;
    [SerializeField] private bool _idleAnimation = false;

    private void Awake() {
        if (_animator == null)
        {
            _animator = GetAnimator();
        }

        if (gameObject.name.Contains("Morgan") || gameObject.name.Contains("Nath")) _angryAnimation = false;
    }

    protected override void OnEnable() {
        base.OnEnable();
        if (_interactable != null && _animator != null)
        {
            _interactable.ConnectedAnimators.Add(_animator);
        }
    }


    protected override void OnDisable() {
        base.OnDisable();
        if (_interactable != null && _animator != null)
        {
            _interactable.ConnectedAnimators.Remove(_animator);
        }
    }

    private void OnValidate() {
        if (_animator == null)
        {
            _animator = GetAnimator();
        }
    }

    public override void Invoke()
    {
        if (_angryAnimation) _animator.SetTrigger("angry");
        else if (_surpriseAnimation) _animator.SetTrigger("surprise");

        else if (_pianoAnimation) _animator.SetTrigger("piano");
        else if (_sleepAnimation) _animator.SetTrigger("sleep");
        else if (_idleAnimation) _animator.SetTrigger("idle");
    }

    private Animator GetAnimator()
    {
        var anim = GetComponent<Animator>();
        if (anim != null) return anim;
        anim = GetComponentInChildren<Animator>();
        if (anim != null) return anim;
        Debug.LogError("Error: No animator attached to this Interaction Response Animator", gameObject);
        return null;
    }
}