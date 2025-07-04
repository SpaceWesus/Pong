using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerLevel9 : GameManager
{
    [Header("Breakout")]
    [SerializeField] private int bricks;
    [SerializeField] private int balls;

    //-------------//
    // LEVEL START //
    //-------------//

    // Initializes the player and opponent scores upon level start.
    protected override void InitializeScores()
    {
        playerScore = balls;
        opponentScore = bricks;
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    //-------------------//
    // BALL MANIPULATION //
    //-------------------//

    // Launches the ball in a semi-random direction.
    protected override void LaunchBall()
    {
        // Determine which of the four diagonal directions the ball should be launched in at random.
        int ballDirection = Mathf.RoundToInt(Random.Range(1, 2));
        Vector2 ballVelocity;
        switch (ballDirection)
        {
            case 1:
                ballVelocity = new Vector2(-initialBallVelocity, initialBallVelocity);
                break;
            case 2:
                ballVelocity = new Vector2(-initialBallVelocity, -initialBallVelocity);
                break;
            default:
                ballVelocity = new Vector2(-initialBallVelocity, initialBallVelocity);
                break;
        }

        // Nudge the ball's launch angle slightly.
        float thetaDegrees = Random.Range(minBallLaunchNudge, maxBallLaunchNudge);
        float thetaRadians = thetaDegrees * (Mathf.PI / 180);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2((ballVelocity.x * Mathf.Cos(thetaRadians)) - (ballVelocity.y * Mathf.Sin(thetaRadians)),
                                        (ballVelocity.x * Mathf.Sin(thetaRadians)) + (ballVelocity.y * Mathf.Cos(thetaRadians)));
    }

    //------------------//
    // WINNING / LOSING //
    //------------------//

    // Handles player scoring.
    public override void PlayerScore()
    {
        opponentScore--;
        opponentScoreText.text = opponentScore.ToString();
        CheckWinCondition();

        // If the player didn't make the winnnig goal, play normal score audio.
        if (!playerWon) audioSource.PlayOneShot(playerScoreAudio);
    }

    // Handles opponent scoring.
    public override void OpponentScore()
    {
        FreezeBall();
        playerScore--;
        playerScoreText.text = playerScore.ToString();
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
    protected override void CheckWinCondition()
    {
        if (opponentScore <= 0)
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
        if (playerScore <= 0)
        {
            opponentWon = true;
            audioSource.PlayOneShot(loseAudio);
            StartCoroutine(Lose());
        }
    }

    // Handles the player winning.
    protected override IEnumerator Win()
    {
        FreezeBall();

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

}
