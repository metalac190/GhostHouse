using Mechanics.Level_Mechanics;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private GameObject _locked = null;
    [SerializeField] private GameObject _unlocked = null;

    private bool _first = true;

    public Interactable Interactable => _interactable;

    private void OnEnable() {
        bool unlocked = _interactable != null && GetUnlocked(_interactable.name);

        if (_locked != null) _locked.gameObject.SetActive(!unlocked);
        if (_unlocked != null) {
            _unlocked.gameObject.SetActive(unlocked);

            if (unlocked) {
                var anim = _unlocked.GetComponent<Animator>();
                if (anim != null) {
                    anim.SetBool("first", _first);
                }
                _first = false;
            }
        }
    }

    private static bool GetUnlocked(string interaction) {
        return DataManager.Instance.journalUnlocks.ContainsKey(interaction) && DataManager.Instance.journalUnlocks[interaction];
    }
}