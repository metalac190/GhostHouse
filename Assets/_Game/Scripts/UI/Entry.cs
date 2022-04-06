using Mechanics.Level_Mechanics;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private GameObject _locked = null;
    [SerializeField] private GameObject _unlocked = null;

    public Interactable Interactable => _interactable;

    private void OnEnable() {
        bool unlocked = _interactable != null && GetUnlocked(_interactable.name);

        if (_locked != null) _locked.gameObject.SetActive(!unlocked);
        if (_unlocked != null) _unlocked.gameObject.SetActive(unlocked);
    }

    private static bool GetUnlocked(string interaction) {
        return DataManager.Instance.interactions.ContainsKey(interaction) && DataManager.Instance.interactions[interaction];
    }
}