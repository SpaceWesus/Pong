using TMPro;
using UnityEngine;

public class GoalLevel10 : MonoBehaviour
{
    [SerializeField] protected GameManagerLevel10 gameManager;
    [SerializeField] protected TMP_Text currentBallCountText;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Player Goal"))
        {
            gameManager.OpponentScore();
            Destroy(collision.gameObject);
        }
        else if (gameObject.CompareTag("Opponent Goal"))
        {
            gameManager.PlayerScore();
            Destroy(collision.gameObject);
        }
    }
}
