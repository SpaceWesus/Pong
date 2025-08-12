using UnityEngine;

public class GoalLevel10 : MonoBehaviour
{
    [SerializeField] protected GameManagerLevel10 gameManager;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Player Goal"))
        {
            gameManager.OpponentScore();
        }
        else if (gameObject.CompareTag("Opponent Goal"))
        {
            gameManager.PlayerScore();
        }
    }
}
