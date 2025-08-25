using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    public void LoadLevel(String level)
    {
        // This function takes in a string that is assigned to the OnClick action assigned to each button 
        // on the level select Menu. 
        Debug.Log(level);
        SceneManager.LoadScene(level);

        // Example: Game Object "Button - Level 1 - Base" -> "Level 1 - BaseGame" which is the name of the first 
        // Level/Scene of this game. 

        // Each level has its own corresponding name and button that needs to be updated with the name of the level 
        // for this to work. 
    }
}
