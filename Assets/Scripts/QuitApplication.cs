using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        // Check every frame if the Escape key is currently being held down
        if (Keyboard.current.escapeKey.isPressed)
        {
            // Print a debug message in the Unity Console (only visible in Editor/Dev builds)
            Debug.Log("We pushed escape, aren't we clever");

            // Quit the application
            // - In the Unity Editor: this does nothing (you'll still see the Debug.Log)
            // - In a built game: this will close the application window
            Application.Quit();
        }
    }
}
