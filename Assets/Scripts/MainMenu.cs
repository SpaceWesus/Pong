using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Can start with "Level 1 - BaseGame" Scene via a string reference
        SceneManager.LoadScene("Level 1");

        // Alternative LoadScene "Methodology"
        // Will be useful when in game already and you want to move on to the next after winning the current level.
        // Can also load the next scene in the order via the build settings in Unity (Next scene would be "Level 1")
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
