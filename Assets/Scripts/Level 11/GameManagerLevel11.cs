using UnityEngine;
using System.Collections;

public class GameManagerLevel11 : GameManager
{
    protected override IEnumerator Countdown()
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
        //  removed the LaunchBall() call that was causing an issue.
    }
}
