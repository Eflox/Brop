/*
 * ReadDebugLog.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 31/07/2024
 * Contact: c.dansembourg@icloud.com
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadDebugLog : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _logText;

    private List<string> logMessages = new List<string>();
    private const int maxLogMessages = 60;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logMessages.Count >= maxLogMessages)
        {
            logMessages.RemoveAt(0);
        }
        logMessages.Add(logString);
        UpdateLogText();
    }

    private void UpdateLogText()
    {
        _logText.text = string.Join("\n - ", logMessages);
    }
}