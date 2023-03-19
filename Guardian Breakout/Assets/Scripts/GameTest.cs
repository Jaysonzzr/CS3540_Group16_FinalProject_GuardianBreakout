using UnityEngine;

public class GameTest : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float fps = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + Mathf.Round(fps));
    }
}
