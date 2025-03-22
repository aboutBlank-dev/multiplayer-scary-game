using UnityEngine;

public class DebugLogManager : MonoBehaviour
{
    // Store the log messages
    private static string _logMessage = "";

    // Call this method to log a message
    public static void Log(string message)
    {
        _logMessage = message;
    }

    // Call this method to display logs using GUI
    void OnGUI()
    {
        GUI.skin.label.fontSize = 20;  // Set the font size for better visibility
        GUI.Label(new Rect(10, 10, Screen.width - 20, Screen.height - 20), _logMessage);
    }
}
