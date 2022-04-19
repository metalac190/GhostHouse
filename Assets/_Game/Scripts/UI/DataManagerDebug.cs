using System.Linq;
using TMPro;
using UnityEngine;

public class DataManagerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private GameObject _parent = null;

    private static bool _debugActive;
    static string myLog = "";
    private string output;
    private string stack;

    private void Start() {
        SetDebugActive(_debugActive);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }
    
    private void OnGUI()
    {
        if (_debugActive) {
            myLog = GUI.TextArea(new Rect(10, 400, 320, Screen.height - 410), myLog);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.End)) {
            SetDebugActive(!_debugActive);
        }
        if (_debugActive) {
            var interactions = DataManager.Instance.interactions;
            string debug = "Season: " + DataManager.Instance.level + "\n";
            debug += "\n<b><u>Spirit Points</u></b>\n";
            debug += "Remaining: " + DataManager.Instance.remainingSpiritPoints + "\n";
            debug += "Total Used: " + DataManager.Instance.totalUsedSpiritPoints + "\n";
            debug += "\n<b><u>Endings</u></b>\n";
            debug += "True: " + DataManager.Instance.trueEndingPoints + "\n";
            debug += "Sisters: " + DataManager.Instance.sistersEndingPoints + "\n";
            debug += "Cousins: " + DataManager.Instance.cousinsEndingPoints + "\n";
            debug += "\n<b><u>Interactions</u></b>\n";
            debug += interactions.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            _text.text = debug;
        }
    }

    public void GiveSpiritPoint() {
        if (DataManager.Instance.remainingSpiritPoints >= 10) return;
        DataManager.Instance.remainingSpiritPoints += 1;
        ModalWindowController.Singleton.ForceUpdateHudSpiritPoints();
    }

    private void SetDebugActive(bool active) {
        _debugActive = active;
        _parent.SetActive(active);
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (myLog.Length > 5000)
        {
            myLog = myLog.Substring(0, 4000);
        }
    }
}
