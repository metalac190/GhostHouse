using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.Level_Mechanics;

public class InteractionPositionSwitch : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private GameObject _preInteractionObject = null;
    [SerializeField] private GameObject _postInteractionObject = null;

    public Interactable Interactable => _interactable;

    private void OnEnable()
    {
        bool unlocked = _interactable != null && GetUnlocked(_interactable.name);

        if (_preInteractionObject != null) _preInteractionObject.gameObject.SetActive(!unlocked);
        if (_postInteractionObject != null) _postInteractionObject.gameObject.SetActive(unlocked);
    }

    private static bool GetUnlocked(string interaction)
    {
        return DataManager.Instance.interactions.ContainsKey(interaction) && DataManager.Instance.interactions[interaction];
    }
}
