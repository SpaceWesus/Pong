using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] protected GameManager gameManager;

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
