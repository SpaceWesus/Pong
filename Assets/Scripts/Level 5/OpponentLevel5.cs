using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class OpponentLevel5 : Opponent
{
    [SerializeField] float minXPos;
    [SerializeField] float maxXPos;

    private Rigidbody2D ballRB;
    protected override void Start()
    {
        base.Start();
        ballRB = ball.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        float yDiff = ball.transform.position.y - transform.position.y;
        float xDiff = ball.transform.position.x - transform.position.y;
        float ballVelX = ballRB.linearVelocityX;
        float newX;

        // Predictive horizontal movement: move towards ball only if it's coming towards the opponent
        float targetX = transform.position.x;
        if ((ballVelX > 0 && ball.transform.position.x > transform.position.x) ||
            (ballVelX < 0 && ball.transform.position.x < transform.position.x))
        {
            targetX = ball.transform.position.x;
        }
        if (ballVelX > 0)
        {
            newX = Mathf.MoveTowards(transform.position.x, maxXPos, (moveSpeed / 3 ) * Time.deltaTime);
        }
        else
        {
            newX = Mathf.MoveTowards(transform.position.x, minXPos, ( moveSpeed / 5 ) * Time.deltaTime);
        }

        // Clamp X position within minXPos and maxXPos
        newX = Mathf.Clamp(newX, minXPos, maxXPos);

        // Apply movement
        transform.position = new Vector3(newX, transform.position.y + ( yDiff * moveSpeed * Time.deltaTime), transform.position.z);
    }


}

// if ball.rigidbody2D.magnitude.x == fast, move back, else move forward. 

/*
protected override void Update()
{
    float yDiff = ball.transform.position.y - transform.position.y;
    float xDiff = ball.transform.position.x - transform.position.x;

    // Calculate new position
    float newX = transform.position.x + (xDiff * Time.deltaTime * moveSpeed);
    float newY = transform.position.y + (yDiff * Time.deltaTime * moveSpeed);

    // Clamp X position within minXPos and maxXPos
    newX = Mathf.Clamp(newX, minXPos, maxXPos);

    // Apply movement
    transform.position = new Vector3(newX, newY, transform.position.z);
}
*/