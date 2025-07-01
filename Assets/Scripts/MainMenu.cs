using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levelSelect;

    [Header("Level Select")]
    [SerializeField] TMP_Text[] levelButtons;
    [SerializeField] string[] unlockedLevelButtonText;

    [Header("Fade")]
    [SerializeField] private Image fade;
    [SerializeField] protected float fadeFrames;
    [SerializeField] protected float fadeDuration;
    private bool fadingIn = false;
    private bool fadingOut = false;

    [Header("Audio")]
    [SerializeField] AudioClip lowBeep;
    [SerializeField] AudioClip highBeep;
    private AudioSource audioSource;

    private void Start()
    {
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>();
        GameData.instance.unlockedLevels[0] = true;
    }

    private void OnEnable()
    {
        UpdateLevelButtonText();
    }

    public void LoadLevel(int levelNumber)
    {
        // This function takes in a string that is assigned to the OnClick action assigned to each button 
        // on the level select Menu. 
        if (!GameData.instance.unlockedLevels[levelNumber - 1]) return;

        audioSource.PlayOneShot(highBeep);
        StartCoroutine(FadeOut("Level " + levelNumber));

        // Example: Game Object "Button - Level 1 - Base" -> "Level 1 - BaseGame" which is the name of the first 
        // Level/Scene of this game.

        // Each level has its own corresponding name and button that needs to be updated with the name of the level 
        // for this to work.
    }

    // Quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Switches between the main menu and level select.
    public void ToggleMenus()
    {
        if (mainMenu.activeSelf)
        {
            audioSource.PlayOneShot(highBeep);
            mainMenu.SetActive(false);
            levelSelect.SetActive(true);
        }
        else
        {
            audioSource.PlayOneShot(lowBeep);
            levelSelect.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    // Instantly fades in the screen.
    protected void FadeInInstant()
    {
        if (!fadingIn && !fadingOut)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
            fade.gameObject.SetActive(false);
        }
    }

    // Fades in the screen.
    protected IEnumerator FadeIn()
    {
        if (!fadingIn && !fadingOut)
        {
            fadingIn = true;
            while (fade.color.a > 0)
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - (1 / fadeFrames));
                yield return new WaitForSeconds(fadeDuration / fadeFrames);
            }
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
            fade.gameObject.SetActive(false);
            fadingIn = false;
        }
    }

    // Instantly fades out the screen and transitions to the given scene.
    protected void FadeOutInstant(string sceneTransition)
    {
        if (!fadingIn && !fadingOut)
        {
            fade.gameObject.SetActive(true);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
            if (sceneTransition != null) SceneManager.LoadScene(sceneTransition);
        }
    }

    // Fades out the screen and transitions to the given scene.
    protected IEnumerator FadeOut(string sceneTransition)
    {
        if (!fadingIn && !fadingOut)
        {
            fadingOut = true;
            fade.gameObject.SetActive(true);
            while (fade.color.a < 1)
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a + (1 / fadeFrames));
                yield return new WaitForSeconds(fadeDuration / fadeFrames);
            }
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
            fadingOut = false;

            if (sceneTransition != null) SceneManager.LoadScene(sceneTransition);
        }
    }

    protected void UpdateLevelButtonText()
    {
        for (int i = 0; i < 10; i++)
        {
            if (GameData.instance.unlockedLevels[i]) levelButtons[i].text = unlockedLevelButtonText[i];
        }
    }
}
