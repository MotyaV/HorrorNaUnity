using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    // Public variable to define the scene to load
    public string sceneName;

    // Called when the button is clicked
    public void LoadScene()
    {
        // Load the scene by name
        SceneManager.LoadScene(sceneName);
    }
}