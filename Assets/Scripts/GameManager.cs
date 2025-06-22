using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float ballForce;

    [SerializeField] TMP_Text countdownText;
    private bool countingDown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Countdown());
        while (!countingDown) { }
        LaunchBall();
    }

    private void LaunchBall()
    {
        ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ballForce, ballForce));
    }

    private IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1);
        countingDown = true;
    }
}
