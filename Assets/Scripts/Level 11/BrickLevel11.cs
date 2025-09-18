using UnityEngine;

public class BrickLevel11 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // destroy ball that hit the brick.
            Destroy(collision.gameObject);

            // Then destroy the brick.
            Destroy(gameObject);
        }
    }
}
