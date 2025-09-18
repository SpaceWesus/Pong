using UnityEngine;

public class GoalLevel11 : MonoBehaviour
{
    [SerializeField] protected GameManagerLevel11 gameManager;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.CompareTag("Ball"))
        {
            return;
        }

        
        if (gameObject.CompareTag("Player Goal"))
        {
            gameManager.OpponentScore();
        }
        else if (gameObject.CompareTag("Opponent Goal"))
        {
            gameManager.PlayerScore();
        }

        
        Destroy(collision.gameObject);
    }
}
