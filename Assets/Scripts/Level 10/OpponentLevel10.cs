using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class OpponentLevel10 : Opponent
{
    [Header("Level 10 References")]
    [SerializeField] protected GameObject ActiveBalls;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Get all balls in play
        Rigidbody2D[] balls = ActiveBalls.GetComponentsInChildren<Rigidbody2D>();
        if (balls.Length == 0) return;

        Rigidbody2D threatBall = null;
        float maxX = float.MinValue;

        foreach (var ball in balls)
        {
            float ballX = ball.transform.position.x;
            float ballVelX = ball.linearVelocityX;

            // Only consider balls moving towards the opponent (x > 0)
            if (ballVelX > 0 && ballX > maxX)
            {
                maxX = ballX;
                threatBall = ball;
            }
        }

        // If no ball is moving towards the opponent, pick the furthest ball anyway
        if (threatBall == null)
        {
            foreach (var ball in balls)
            {
                float ballX = ball.transform.position.x;
                if (ballX > maxX)
                {
                    maxX = ballX;
                    threatBall = ball;
                }
            }
        }

        if (threatBall != null)
            Defense(threatBall.gameObject);
    }

    void Defense(GameObject currentThreat)
    {  
        float yDiff = currentThreat.transform.position.y - transform.position.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + (yDiff * Time.deltaTime * moveSpeed), transform.position.z);
    }

}
