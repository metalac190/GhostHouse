using Mechanics.Level_Mechanics;
using UnityEngine;

public class JournalHint : MonoBehaviour
{
    [SerializeField] private Season _season = Season.None;
    [SerializeField] private Interactable _showAfter = null;
    [SerializeField] private Interactable _showUntil = null;
    [SerializeField] private Interactable _showUntilAlt = null;
    [SerializeField] private Interactable _showUntilAlt2 = null;
    [SerializeField] private GameObject _hint = null;
    [SerializeField] private bool _debug = false;

    public Interactable InteractableShowAfter => _showAfter;
    public Interactable InteractableShowUntil => _showUntil;

    private bool _first = true;

    private void OnEnable() {
        var season = DataManager.Instance.GetSeason();
        bool seasonCheck = _season == season || season == Season.Universal;

        bool after = _showAfter == null || GetUnlocked(_showAfter.name);
        bool until = _showUntil != null && GetUnlocked(_showUntil.name);
        bool untilAlt = _showUntilAlt != null && GetUnlocked(_showUntilAlt.name);
        bool untilAlt2 = _showUntilAlt2 != null && GetUnlocked(_showUntilAlt2.name);

        bool unlocked = seasonCheck && after && !(until || untilAlt || untilAlt2);
        if (_debug) {
            Debug.Log("Journal Hint (" + _hint.name + ") is " + (unlocked ? "" : "not ") + "unlocked");
            Debug.Log("  - Season (" + seasonCheck + " -- Set to " + _season + " and currently in " + season + "), After (" + after + "), Until (" + until + "or " + untilAlt + ")");
        }

        if (_hint != null) {
            _hint.gameObject.SetActive(unlocked);

            if (unlocked) {
                var anim = _hint.GetComponent<Animator>();
                if (anim != null) {
                    anim.SetBool("first", _first);
                }
                _first = false;
            }
        }
    }

    private static bool GetUnlocked(string interaction) {
        return DataManager.Instance.interactions.ContainsKey(interaction) && DataManager.Instance.interactions[interaction];
    }
}