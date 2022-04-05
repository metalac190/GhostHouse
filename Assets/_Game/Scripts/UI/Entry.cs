using Mechanics.Level_Mechanics;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private GameObject _locked = null;
    [SerializeField] private GameObject _unlocked = null;

    private void OnEnable() {
        if (_interactable == null) return;
        bool unlocked = DataManager.Instance.interactions[_interactable.name];

        if (_locked != null) _locked.gameObject.SetActive(!unlocked);
        if (_unlocked != null) _unlocked.gameObject.SetActive(unlocked);
    }
}
