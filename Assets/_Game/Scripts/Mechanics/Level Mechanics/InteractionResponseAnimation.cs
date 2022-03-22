using UnityEngine;

public class InteractionResponseAnimation : InteractableResponseBase
{
    public override void Invoke() {
        var anim = GetAnimator();
        anim.SetTrigger("interact");
    }

    private Animator GetAnimator()
    {
        var parent = gameObject.transform.parent;
        var anim = parent.GetComponent<Animator>();
        if (anim != null) return anim;
        anim = parent.GetComponentInChildren<Animator>();
        if (anim != null) return anim;
        Debug.LogError("Error: No animator attached to this Interaction Response Animator", gameObject);
        return null;
    }
}
