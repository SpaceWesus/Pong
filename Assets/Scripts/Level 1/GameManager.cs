using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] protected int levelNumber;

    [Header("Ball Settings")]
    [SerializeField] protected float initialBallVelocity;
    [SerializeField] protected float minBallLaunchNudge;
    [SerializeField] protected float maxBallLaunchNudge;

    [Header("UI Settings")]
    [SerializeField] protected string winConditionString;
    [SerializeField] protected string loseConditionString;
    [SerializeField] protected float fadeFrames;
    [SerializeField] protected float fadeDuration;
    [SerializeField] protected float winTextDisplayTime;
    [SerializeField] protected float loseTextDisplayTime;

    [Header("Object References")]
    [SerializeField] protected GameObject ball;
    [SerializeField] protected TMP_Text gameStatusText;
    [SerializeField] protected TMP_Text playerScoreText;
    [SerializeField] protected TMP_Text opponentScoreText;
    [SerializeField] protected TMP_Text winConditionText;
    [SerializeField] protected TMP_Text loseConditionText;
    [SerializeField] protected Image fade;

    [Header("Audio")]
    [SerializeField] protected AudioClip playerScoreAudio;
    [SerializeField] protected AudioClip opponentScoreAudio;
    [SerializeField] protected AudioClip winAudio;
    [SerializeField] protected AudioClip loseAudio;
    [SerializeField] protected AudioClip countdownAudio;
    [SerializeField] protected AudioClip goAudio;
    protected AudioSource audioSource;

    protected Vector2 initialBallPos;
    protected int playerScore;
    protected int opponentScore;
    protected bool playerWon = false;
    protected bool opponentWon = false;

    protected bool fadingIn = false;
    protected bool fadingOut = false;

    //--------------//
    // MAIN METHODS //
    //--------------//

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeScores();
        UpdateWinLoseConditionText();
        StartCoroutine(FadeIn(true));
        initialBallPos = ball.transform.position;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) ReturnToMainMenu();
    }

    //-------------//
    // LEVEL START //
    //-------------//

    // Initializes the player and opponent scores upon level start.
    protected virtual void InitializeScores()
    {
        playerScore = 0;
        opponentScore = 0;
    }

    // Updates the win and lose condition text upon level start.
    protected virtual void UpdateWinLoseConditionText()
    {
        winConditionText.text = "WIN CONDITION: " + winConditionString;
        loseConditionText.text = "LOSE CONDITION: " + loseConditionString;
    }

    // Handles the countdown before the ball is launched.
    protected virtual IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);
        gameStatusText.text = "3";
        audioSource.PlayOneShot(countdownAudio);
        yield return new WaitForSeconds(1);
        gameStatusText.text = "2";
        audioSource.PlayOneShot(countdownAudio);
        yield return new WaitForSeconds(1);
        gameStatusText.text = "1";
        audioSource.PlayOneShot(countdownAudio);
        yield return new WaitForSeconds(1);
        gameStatusText.text = "GO!";
        audioSource.PlayOneShot(goAudio);
        yield return new WaitForSeconds(1);
        gameStatusText.text = "";
        LaunchBall();
    }

    //-------------------//
    // BALL MANIPULATION //
    //-------------------//

    // Launches the ball in a semi-random direction.
    protected virtual void LaunchBall()
    {
        // Determine which of the four diagonal directions the ball should be launched in at random.
        int ballDirection = Mathf.RoundToInt(Random.Range(1, 4));
        Vector2 ballVelocity;
        switch (ballDirection)
        {
            case 1:
                ballVelocity = new Vector2(initialBallVelocity, initialBallVelocity);
                break;
            case 2:
                ballVelocity = new Vector2(initialBallVelocity, -initialBallVelocity);
                break;
            case 3:
                ballVelocity = new Vector2(-initialBallVelocity, initialBallVelocity);
                break;
            case 4:
                ballVelocity = new Vector2(-initialBallVelocity, -initialBallVelocity);
                break;
            default:
                ballVelocity = new Vector2(initialBallVelocity, initialBallVelocity);
                break;
        }

        // Nudge the ball's launch angle slightly.
        float thetaDegrees = Random.Range(minBallLaunchNudge, maxBallLaunchNudge);
        float thetaRadians = thetaDegrees * (Mathf.PI / 180);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2((ballVelocity.x * Mathf.Cos(thetaRadians)) - (ballVelocity.y * Mathf.Sin(thetaRadians)),
                                        (ballVelocity.x * Mathf.Sin(thetaRadians)) + (ballVelocity.y * Mathf.Cos(thetaRadians)));
    }

    // Freezes the ball in place.
    protected virtual void FreezeBall()
    {
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    // Resets the ball's position to its initial position at the start of the level.
    protected virtual void ResetBall()
    {
        FreezeBall();
        ball.transform.position = initialBallPos;
        StartCoroutine(Countdown());
    }

    //------------------//
    // WINNING / LOSING //
    //------------------//

    // Handles player scoring.
    public virtual void PlayerScore()
    {
        FreezeBall();
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        CheckWinCondition();

        // If the player didn't make the winnnig goal, reset for the next round.
        if (!playerWon)
        {
            audioSource.PlayOneShot(playerScoreAudio);
            ResetBall();
        }
    }

    // Handles opponent scoring.
    public virtual void OpponentScore()
    {
        FreezeBall();
        opponentScore++;
        opponentScoreText.text = opponentScore.ToString();
        CheckLoseCondition();

        // If the opponent didn't make the winning goal, reset for the next round.
        if (!opponentWon)
        {
            audioSource.PlayOneShot(opponentScoreAudio);
            ResetBall();
        }
    }

    // Checks if the win condision for this level has been met.
    // OVERRIDE THIS FUNCTION IN AN INHERETING SCRIPT TO CHANGE THE WIN CONDITION!
    protected virtual void CheckWinCondition()
    {
        if (playerScore >= 3)
        {
            playerWon = true;
            audioSource.PlayOneShot(winAudio);
            StartCoroutine(Win());
        }
    }

    // Checks if the lose condision for this level has been met.
    // OVERRIDE THIS FUNCTION IN AN INHERETING SCRIPT TO CHANGE THE LOSE CONDITION!
    protected virtual void CheckLoseCondition()
    {
        if (opponentScore >= 3)
        {
            opponentWon = true;
            audioSource.PlayOneShot(loseAudio);
            StartCoroutine(Lose());
        }
    }

    // Handles the player winning.
    protected virtual IEnumerator Win()
    {
        // Show the victory text for a few seconds.
        gameStatusText.text = "YOU WIN!";
        yield return new WaitForSeconds(winTextDisplayTime);

        // Update the next level.
        if (levelNumber < 10) GameData.instance.unlockedLevels[levelNumber] = true;

        // Fade out the screen and advance to the next level.
        int nextLevel = levelNumber + 1;
        if (levelNumber == 10) StartCoroutine(FadeOut("Main Menu"));
        else StartCoroutine(FadeOut("Level " + nextLevel));
    }

    // Handles the opponent winning.
    protected virtual IEnumerator Lose()
    {
        // Show the lose text for a few seconds.
        gameStatusText.text = "YOU LOSE!";
        yield return new WaitForSeconds(loseTextDisplayTime);

        // Fade out the screen and reload the current scene.
        StartCoroutine(FadeOut("Level " + levelNumber));
    }

    //--------//
    // FADING //
    //--------//

    // Instantly fades in the screen and starts the countdown if necessary.
    protected virtual void FadeInInstant(bool startCountdown)
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        fade.gameObject.SetActive(false);
        if (startCountdown) StartCoroutine(Countdown());
    }

    // Fades in the screen and starts the countdown if necessary.
    protected virtual IEnumerator FadeIn(bool startCountdown)
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
    }

    // Instantly fades out the screen and transitions to the given scene.
    protected virtual void FadeOutInstant(string sceneTransition)
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
        if (sceneTransition != null) SceneManager.LoadScene(sceneTransition);
    }

    // Fades out the screen and transitions to the given scene.
    protected virtual IEnumerator FadeOut(string sceneTransition)
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

    //-------//
    // OTHER //
    //-------//

    // Returns the player to the main menu as long as level completion isn't processing.
    protected virtual void ReturnToMainMenu()
    {
        if (fadingIn || fadingOut || playerWon || opponentWon) return;
        FreezeBall();
        StartCoroutine(FadeOut("Main Menu"));
    }

}
