using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutput : MonoBehaviour
{
    static string myLog = "";
    private string output;
    private string stack;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
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

    private void OnGUI()
    {
        {
            myLog = GUI.TextArea(new Rect(10, 10, 320, Screen.height - 10), myLog);
        }
    }
}
