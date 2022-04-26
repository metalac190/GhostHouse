using UnityEngine;

public class InteractionResponseAnimation : InteractableResponseBase
{
    [SerializeField] private Animator _animator;

    private void Awake() {
        if (_animator == null)
        {
            _animator = GetAnimator();
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        if (_interactable != null && _animator != null)
        {
            _interactable.TestResetAnimators();
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
        _animator.SetTrigger("interact");
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