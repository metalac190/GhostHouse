using Mechanics.Level_Mechanics;
using UnityEngine;

public abstract class InteractableResponseBase : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    
    private void OnEnable()
    {
        if (_interactable == null) return;
        _interactable.Raise(this);
    }

    private void Start()
    {
        if (_interactable == null) return;
        _interactable.LoadInteraction();
    }

    private void OnDisable()
    {
        if (_interactable == null) return;
        _interactable.Unraise(this);
    }

    public abstract void Invoke();
}