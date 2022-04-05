using Mechanics.Level_Mechanics;
using UnityEngine;

public abstract class InteractableResponseBase : MonoBehaviour
{
    [SerializeField] protected Interactable _interactable = null;

    public Interactable Interactable => _interactable;
    
    protected virtual void OnEnable()
    {
        if (_interactable == null) return;
        _interactable.Raise(this);
    }

    protected virtual void Start()
    {
        if (_interactable == null) return;
        // TODO: This doesn't work
        //if (_interactable.Interacted) PreviouslyInvoked();
    }

    protected virtual void OnDisable()
    {
        if (_interactable == null) return;
        _interactable.Unraise(this);
    }

    public abstract void Invoke();

    public virtual void PreviouslyInvoked()
    {

    }
}