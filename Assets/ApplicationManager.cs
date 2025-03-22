using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Application.targetFrameRate = 300;
    }
}
