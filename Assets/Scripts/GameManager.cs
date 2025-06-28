using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] protected float ballForce;
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
    [SerializeField] Image fade;

    protected Vector2 initialBallPos;
    protected int playerScore;
    protected int opponentScore;
    protected bool playerWon = false;
    protected bool opponentWon = false;

    protected bool fadingIn = false;
    protected bool fadingOut = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        InitializeScores();
        UpdateWinLoseConditionText();
        StartCoroutine(FadeIn(true));
        initialBallPos = ball.transform.position;
    }

    protected virtual void InitializeScores()
    {
        playerScore = 0;
        opponentScore = 0;
    }

    protected virtual void UpdateWinLoseConditionText()
    {
        winConditionText.text = "WIN CONDITION: " + winConditionString;
        loseConditionText.text = "LOSE CONDITION: " + loseConditionString;
    }

    protected virtual void LaunchBall()
    {
        if (Random.Range(-1f, 1f) > 0f)
        {
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(ballForce, ballForce), ForceMode2D.Impulse);
        } else {
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ballForce, ballForce), ForceMode2D.Impulse);
        }

    }

    public virtual void PlayerScore()
    {
        FreezeBall();
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        CheckWinCondition();
        if (!playerWon) ResetBall();
    }

    public virtual void OpponentScore()
    {
        FreezeBall();
        opponentScore++;
        opponentScoreText.text = opponentScore.ToString();
        CheckLoseCondition();
        if (!opponentWon) ResetBall();
    }

    protected virtual void FreezeBall()
    {
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    protected virtual void ResetBall()
    {
        FreezeBall();
        ball.transform.position = initialBallPos;
        StartCoroutine(Countdown());
    }

    protected virtual void CheckWinCondition()
    {
        if (playerScore >= 3)
        {
            playerWon = true;
            StartCoroutine(Win());
        }
    }

    protected virtual void CheckLoseCondition()
    {
        if (opponentScore >= 3)
        {
            opponentWon = true;
            StartCoroutine(Lose());
        }
    }

    protected void FadeInInstant(bool startCountdown)
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        fade.gameObject.SetActive(false);
        if (startCountdown) StartCoroutine(Countdown());
    }

    protected IEnumerator FadeIn(bool startCountdown)
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

    protected void FadeOutInstant(string sceneTransition)
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
        if (sceneTransition != null) SceneManager.LoadScene(sceneTransition);
    }

    protected IEnumerator FadeOut(string sceneTransition)
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

    protected virtual IEnumerator Countdown()
    {
        gameStatusText.text = "3";
        yield return new WaitForSeconds(1);
        gameStatusText.text = "2";
        yield return new WaitForSeconds(1);
        gameStatusText.text = "1";
        yield return new WaitForSeconds(1);
        gameStatusText.text = "GO!";
        yield return new WaitForSeconds(1);
        gameStatusText.text = "";
        LaunchBall();
    }

    protected virtual IEnumerator Win()
    {
        // Show the victory text for a few seconds.
        gameStatusText.text = "YOU WIN!";
        yield return new WaitForSeconds(winTextDisplayTime);

        // Fade out the screen and advance to the next level.
        StartCoroutine(FadeOut("Level 1"));
    }

    protected virtual IEnumerator Lose()
    {
        // Show the lose text for a few seconds.
        gameStatusText.text = "YOU LOSE!";
        yield return new WaitForSeconds(loseTextDisplayTime);

        // Fade out the screen and reload the current scene.
        StartCoroutine(FadeOut("Level 1"));
    }
}
