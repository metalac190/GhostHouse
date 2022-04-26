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
        if (_angryAnimation && !_surpriseAnimation) _animator.SetTrigger("angry");
        if (_surpriseAnimation && !_angryAnimation) _animator.SetTrigger("surprise");

        if (!_angryAnimation && !_surpriseAnimation && !_sleepAnimation) _animator.SetTrigger("piano");
        if (!_angryAnimation && !_surpriseAnimation && !_pianoAnimation) _animator.SetTrigger("sleep");
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