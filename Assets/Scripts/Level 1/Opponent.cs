using UnityEngine;

public class Opponent : MonoBehaviour
{
    [SerializeField] protected GameObject ball;
    [SerializeField] protected float moveSpeed;

    protected Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        // OPPONENT AI 1: Opponent perfectly matches ball's y position.
        // PROBLEMS:      Impossible to beat.
        /*
        transform.position = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        */

        // OPPONENT AI 2: Add force based on whether the ball is above or below the opponent.
        // PROBLEMS:      The opponent almost always misses the ball.
        /*
        if (ball.transform.position.y > rb.transform.position.y)
        {
            rb.AddForce(Vector2.up);
        } else {rb.AddForce(Vector2.down);}
        */

        // OPPONENT AI 3: Add force proportional to the difference in the ball and opponent's y positions.
        // PROBLEMS:      Opponent can start "orbiting" up and down relative to the ball.
        /*
        float yDiff = Mathf.Abs(ball.transform.position.y - rb.transform.position.y);
        float scaler = 0.05f;
        if (ball.transform.position.y > rb.transform.position.y)
        {
            rb.AddForce(Vector2.up * yDiff * scaler);
        }
        else { rb.AddForce(Vector2.down * yDiff * scaler); }
        */

        // OPPONENT AI 4: Manually translate ball with deltatime, increasing speed with increased y difference.
        // PROBLEMS:      Opponent can start to jitter in place after a goal.
        float yDiff = ball.transform.position.y - transform.position.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + (yDiff * Time.deltaTime * moveSpeed), transform.position.z);
    }
}
