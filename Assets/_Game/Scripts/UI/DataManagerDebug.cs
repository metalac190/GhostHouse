using System.Linq;
using TMPro;
using UnityEngine;

public class DataManagerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private GameObject _parent = null;

    private static bool _debugActive;

    private void Start() {
        SetDebugActive(_debugActive);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.End)) {
            SetDebugActive(!_debugActive);
        }
        if (_debugActive) {
            var interactions = DataManager.Instance.interactions;
            string debug = "Debug\n";
            debug += interactions.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            debug += "\nEndings\n";
            debug += "True: " + DataManager.Instance.trueEndingPoints + "\n";
            debug += "Sisters: " + DataManager.Instance.sistersEndingPoints + "\n";
            debug += "Cousins: " + DataManager.Instance.cousinsEndingPoints + "\n";
            _text.text = debug;
        }
    }

    private void SetDebugActive(bool active) {
        _debugActive = active;
        _parent.SetActive(active);
    }
}
