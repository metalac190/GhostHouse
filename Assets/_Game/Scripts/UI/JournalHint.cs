using Mechanics.Level_Mechanics;
using UnityEngine;

public class JournalHint : MonoBehaviour
{
    [SerializeField] private Season _season = Season.None;
    [SerializeField] private Interactable _showAfter = null;
    [SerializeField] private Interactable _showUntil = null;
    [SerializeField] private GameObject _hint = null;

    public Interactable InteractableShowAfter => _showAfter;
    public Interactable InteractableShowUntil => _showUntil;

    private void OnEnable() {
        bool after = _showAfter != null && GetUnlocked(_showAfter.name);
        bool until = _showUntil != null && GetUnlocked(_showUntil.name);

        bool unlocked = after && !until;

        if (_hint != null) _hint.gameObject.SetActive(unlocked);
    }

    private static bool GetUnlocked(string interaction) {
        return DataManager.Instance.interactions.ContainsKey(interaction) && DataManager.Instance.interactions[interaction];
    }
}
