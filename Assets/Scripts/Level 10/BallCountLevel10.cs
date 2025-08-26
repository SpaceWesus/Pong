using UnityEngine;
using TMPro;

public class BallCountLevel10 : MonoBehaviour
{
    [SerializeField] private TMP_Text ballCountText;

    void Update()
    {
        int ballCount = transform.childCount;
        ballCountText.text = "Active Balls: " + ballCount + " / 3";
    }
}
