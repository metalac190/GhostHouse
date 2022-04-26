using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections;

public class DataManagerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private GameObject _parent = null;
    [SerializeField] private TextMeshProUGUI _timerMain = null;
    [SerializeField] private TextMeshProUGUI _timerMil = null;

    private static bool _debugActive;
    static string myLog = "";
    private string output;
    private string stack;
    private float fps;
    private float updateInterval = 0.5f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;
    private float startTime;

    private void Start() {
        startTime = Time.time;
        SetDebugActive(_debugActive);
    }

    private void OnEnable() {
        Application.logMessageReceived += Log;
    }

    private void OnDisable() {
        Application.logMessageReceived -= Log;
    }

    private void OnGUI() {
        if (_debugActive) {
            myLog = GUI.TextArea(new Rect(10, 420, 320, Screen.height - 430), myLog);
            GUI.Label(new Rect(10, 10, 120, 32), "FPS: " + fps);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.End)) {
            SetDebugActive(!_debugActive);
        }
        if (_debugActive) {
            string debug = "Season: " + DataManager.Instance.level + "\n";
            debug += "\n<b><u>Spirit Points</u></b>\n";
            debug += "Remaining: " + DataManager.Instance.remainingSpiritPoints + "\n";
            debug += "Total Used: " + DataManager.Instance.totalUsedSpiritPoints + "\n";
            debug += "\n<b><u>Endings</u></b>\n";
            debug += "True: " + DataManager.Instance.trueEndingPoints + "\n";
            debug += "Sisters: " + DataManager.Instance.sistersEndingPoints + "\n";
            debug += "Cousins: " + DataManager.Instance.cousinsEndingPoints + "\n";
            debug += "\n<b><u>Interactions</u></b>\n";
            debug += DataManager.Instance.interactions.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            debug += "\n<b><u>Journal Unlocks</u></b>\n";
            debug += DataManager.Instance.journalUnlocks.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            _text.text = debug;

            var currentTime = Time.time - startTime;
            float hour = Mathf.FloorToInt(currentTime / 3600);
            float min = Mathf.FloorToInt(currentTime / 60);
            float sec = Mathf.FloorToInt(currentTime % 60);
            _timerMain.text = $"{hour:0}:{min:00}:{sec:00}";
            float ms = (currentTime % 1) * 1000;
            _timerMil.text = $".{ms:000}";
        }
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0) {
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
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

    public void Log(string logString, string stackTrace, LogType type) {
        output = logString;
        stack = stackTrace;
        myLog = output + "\n" + myLog;
        if (myLog.Length > 5000) {
            myLog = myLog.Substring(0, 4000);
        }
    }
}