using System.Linq;
using TMPro;
using UnityEngine;
using Game;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DataManagerDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugText = null;
    [SerializeField] private TextMeshProUGUI _outputLog = null;
    [SerializeField] private GameObject _parent = null;
    [SerializeField] private Season _season = Season.None;
    [SerializeField] private TextMeshProUGUI _timerMain = null;
    [SerializeField] private TextMeshProUGUI _timerMil = null;
    [SerializeField] private TextMeshProUGUI _fpsText = null;
    [SerializeField] private TextMeshProUGUI _perspButtonText = null;

    // Debugging
    private static bool _debugActive;
    private static string _myLog = "Error Logging:";

    // Speedrun stats / fps
    private float _updateInterval = 0.5f;
    private float _fps;
    private float _accum = 0.0f;
    private int _frames = 0;
    private float _timeLeft;
    private float _currentTime;
    private float _holdTime = 0;

    // Perspective Shifts
    private static int _perspLevel = 0;
    private static Vector3 _persp0Pos = new Vector3(0, 0, -100);
    private static float _persp0Len = 60;
    private static Vector3 _persp1Pos = new Vector3(0, 0, -580);
    private static float _persp1Len = 3;
    private static Vector3 _persp2Pos = new Vector3(0, 0, -100);
    private static float _persp2Len = 16;
    private static Vector3 _persp3Pos = new Vector3(0, 2, -16);
    private static float _persp3Len = 65;
    private static AmbientOcclusion _origAO;
    private static AmbientOcclusion _perspAO;

    private void Awake() {
        if (_season == Season.End) {
            var endManager = FindObjectOfType<EndingsManager>();
            if (endManager != null) {
                endManager.OnEnd += SetEndTime;
            }
        }
    }

    private void Start() {
        _currentTime = 0;
        SetDebugActive(_debugActive);
        CheckPerspectiveLevel();
    }

    private void OnEnable() {
        Application.logMessageReceived += Log;
        TransitionManager.OnLevelComplete += SaveSplit;
    }

    private void OnDisable() {
        Application.logMessageReceived -= Log;
        TransitionManager.OnLevelComplete -= SaveSplit;
    }

    private void Update() {
        _currentTime += Time.deltaTime;
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;

        if (_timeLeft <= 0.0) {
            _fps = (_accum / _frames);
            _timeLeft = _updateInterval;
            _accum = 0.0f;
            _frames = 0;
        }

        if (_holdTime == 0) {
            if (_timerMain != null) _timerMain.text = TimeMain(_currentTime);
            if (_timerMil != null) _timerMil.text = TimeMil(_currentTime);
        }
        else {
            if (_timerMain != null) _timerMain.text = TimeMain(_holdTime);
            if (_timerMil != null) _timerMil.text = TimeMil(_holdTime);
        }

        if (Input.GetKeyDown(KeyCode.End)) {
            SetDebugActive(!_debugActive);
        }

        if (_debugActive && _debugText != null) {
            string debug = "Season: " + DataManager.Instance.level + "\n";
            debug += "\n<b><u>Spirit Points</u></b>\n";
            debug += "Remaining: " + DataManager.Instance.remainingSpiritPoints + "\n";
            debug += "Total Used: " + DataManager.Instance.totalUsedSpiritPoints + "\n";
            debug += "\n<b><u>Endings</u></b>\n";
            debug += "True: " + DataManager.Instance.trueEndingPoints + "\n";
            debug += "Sisters: " + DataManager.Instance.sistersEndingPoints + "\n";
            debug += "Cousins: " + DataManager.Instance.cousinsEndingPoints + "\n";
            debug += "\n<b><u>Current Run</u></b>\n";
            debug += "Spring: " + TimeTotal(DataManager.Instance.SpringSplit) + "\n";
            debug += "Summer: " + TimeTotal(DataManager.Instance.SummerSplit) + "\n";
            debug += "Fall: " + TimeTotal(DataManager.Instance.FallSplit) + "\n";
            debug += "Winter: " + TimeTotal(DataManager.Instance.WinterSplit) + "\n";
            debug += "Total: " + TimeTotal(DataManager.Instance.SplitTotal) + "\n";
            debug += "\n<b><u>Best Endings</u></b>\n";
            debug += "True: " + TimeTotal(DataManager.Instance.TrueEndBest) + "\n";
            debug += "Sister: " + TimeTotal(DataManager.Instance.SisterEndBest) + "\n";
            debug += "Cousin: " + TimeTotal(DataManager.Instance.CousinEndBest) + "\n";
            debug += "Bad: " + TimeTotal(DataManager.Instance.BadEndBest) + "\n";
            debug += "\n<b><u>Best Splits</u></b>\n";
            debug += "Best Spring: " + TimeTotal(DataManager.Instance.SpringSplitBest) + "\n";
            debug += "Best Summer: " + TimeTotal(DataManager.Instance.SummerSplitBest) + "\n";
            debug += "Best Fall: " + TimeTotal(DataManager.Instance.FallSplitBest) + "\n";
            debug += "Best Winter: " + TimeTotal(DataManager.Instance.WinterSplitBest) + "\n";
            debug += "\n<b><u>Interactions</u></b>\n";
            debug += DataManager.Instance.interactions.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            debug += "\n<b><u>Journal Unlocks</u></b>\n";
            debug += DataManager.Instance.journalUnlocks.Aggregate("", (current, interaction) => current + (interaction.Key + " - " + interaction.Value + "\n"));
            _debugText.text = debug;
        }

        if (_outputLog != null) _outputLog.text = _myLog;
        if (_fpsText != null) _fpsText.text = "FPS: " + _fps;
    }

    private void SetEndTime(string end) {
        _holdTime = DataManager.Instance.SplitTotal;
        end = end.ToLower();
        if (end.Contains("true")) {
            DataManager.Instance.SetTrueEnd(_holdTime);
        }
        else if (end.Contains("sister")) {
            DataManager.Instance.SetSisterEnd(_holdTime);
        }
        else if (end.Contains("cousin")) {
            DataManager.Instance.SetCousinEnd(_holdTime);
        }
        else if (end.Contains("bad")) {
            DataManager.Instance.SetBadEnd(_holdTime);
        }
    }

    private static string TimeTotal(float time) {
        return TimeMain(time) + TimeMil(time);
    }

    private static string TimeMain(float time) {
        float hour = Mathf.FloorToInt(time / 3600);
        float min = Mathf.FloorToInt(time / 60);
        float sec = Mathf.FloorToInt(time % 60);
        return $"{hour:0}:{min:00}:{sec:00}";
    }

    private static string TimeMil(float time) {
        float ms = (time % 1) * 1000;
        return $".{ms:000}";
    }

    private void SaveSplit() {
        DataManager.Instance.SetSplit(_season, _currentTime);
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
        _myLog = _myLog + "\n" + logString;
    }

    public void LoopPerspectiveLevel() {
        int level = _perspLevel;
        level++;
        if (level > 3) level = 0;
        SetPerspectiveLevel(level);
    }

    // 0 = Orthographic (Normal), 1 = Min Persp, 2 = Med Persp, 3 = High Persp
    public void SetPerspectiveLevel(int level) {
        _perspLevel = level;
        CheckPerspectiveLevel();
    }

    private void CheckPerspectiveLevel() {
        var cam = Camera.main;
        if (cam == null) return;
        //CheckAmbientOcclusion();
        cam.orthographic = _perspLevel == 0;
        switch (_perspLevel) {
            case 1:
                cam.fieldOfView = _persp1Len;
                cam.gameObject.transform.localPosition = _persp1Pos;
                if (_perspButtonText != null) _perspButtonText.text = "Perspective Low";
                break;
            case 2:
                cam.fieldOfView = _persp2Len;
                cam.gameObject.transform.localPosition = _persp2Pos;
                if (_perspButtonText != null) _perspButtonText.text = "Perspective Mid";
                break;
            case 3:
                cam.fieldOfView = _persp3Len;
                cam.gameObject.transform.localPosition = _persp3Pos;
                if (_perspButtonText != null) _perspButtonText.text = "Perspective High";
                break;
            default:
                cam.fieldOfView = _persp0Len;
                cam.gameObject.transform.localPosition = _persp0Pos;
                if (_perspButtonText != null) _perspButtonText.text = "Reg Orthographic";
                break;
        }
    }

    /* BUG: THIS IS BROKEN
    private static void CheckAmbientOcclusion() {
        if (GraphicsController.Instance == null) {
            Debug.Log("Graphics Controller Instance is Null");
            return;
        }
        var profile = GraphicsController.Instance.GetComponent<Volume>()?.sharedProfile;
        if (profile == null) {
            Debug.Log("Post Process Volume Controller is Null");
            return;
        }
        if (profile.TryGet(typeof(AmbientOcclusion), out AmbientOcclusion ambOcc)) {
            Debug.Log("Successfully got Ambient Occlusion");
            if (_perspLevel != 0) {
                ambOcc.intensity = new ClampedFloatParameter(4f, 0f, 4f, true);
                ambOcc.directLightingStrength = new ClampedFloatParameter(1f, 0f, 1f, true);
                ambOcc.intensity = new ClampedFloatParameter(0.75f, 0f, 5f, true);
            }
            else {
                ambOcc.intensity = new ClampedFloatParameter(1f, 0f, 4f, true);
                ambOcc.directLightingStrength = new ClampedFloatParameter(1f, 0f, 1f, true);
                ambOcc.intensity = new ClampedFloatParameter(1f, 0f, 5f, true);
            }
        }
    }
    */
}