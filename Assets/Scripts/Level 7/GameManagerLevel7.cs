using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class GameManagerLevel7 : GameManager
{
    [Header("Time Limit")]
    [SerializeField] private int seconds;
    [SerializeField] private int scoreToWin;

    //-------------//
    // LEVEL START //
    //-------------//

    // Updates the win and lose condition text upon level start.
    protected override void UpdateWinLoseConditionText()
    {
        winConditionText.text = "WIN CONDITION: Score " + scoreToWin + " goals in " + seconds + " seconds!";
        loseConditionText.text = "LOSE CONDITION: " + loseConditionString;
    }

    // Handles the countdown before the ball is launched.
    protected override IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);
        if (!playerWon && !opponentWon) LaunchBall();
    }

    //------------------//
    // WINNING / LOSING //
    //------------------//

    // Checks if the win condision for this level has been met.
    // OVERRIDE THIS FUNCTION IN AN INHERETING SCRIPT TO CHANGE THE WIN CONDITION!
    protected override void CheckWinCondition()
    {
        if (playerScore >= scoreToWin && !opponentWon)
        {
            playerWon = true;
            audioSource.PlayOneShot(winAudio);
            StartCoroutine(Win());
        }
    }

    // Checks if the lose condision for this level has been met.
    // OVERRIDE THIS FUNCTION IN AN INHERETING SCRIPT TO CHANGE THE LOSE CONDITION!
    protected override void CheckLoseCondition()
    {
        
    }

    //--------//
    // FADING //
    //--------//

    // Instantly fades in the screen and starts the countdown if necessary.
    protected override void FadeInInstant(bool startCountdown)
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        fade.gameObject.SetActive(false);
        if (startCountdown) StartCoroutine(Countdown());
        if (startCountdown) StartCoroutine(Timer());
    }

    // Fades in the screen and starts the countdown if necessary.
    protected override IEnumerator FadeIn(bool startCountdown)
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

        if (startCountdown) StartCoroutine(Countdown());
        if (startCountdown) StartCoroutine(Timer());
    }

    //-------//
    // OTHER //
    //-------//

    private IEnumerator Timer()
    {
        for (int i = seconds; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            if (playerWon || fadingOut) break;
            gameStatusText.text = i.ToString();
        }
        if (!playerWon && !fadingOut) yield return new WaitForSeconds(1);
        if (!playerWon && !fadingOut) gameStatusText.text = "0";
        if (!playerWon && !fadingOut) yield return new WaitForSeconds(1);
        if (!playerWon && !fadingOut) opponentWon = true;
        if (!playerWon && !fadingOut) audioSource.PlayOneShot(loseAudio);
        if (!playerWon && !fadingOut) StartCoroutine(Lose());
    }

}
