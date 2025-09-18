using UnityEngine;

public class OpponentLevel11 : OpponentLevel10
{
    protected override void Update()
    {
        // Get all balls in play
        Rigidbody2D[] balls = ActiveBalls.GetComponentsInChildren<Rigidbody2D>();
        if (balls.Length == 0)
        {
            return;
        }

        Rigidbody2D threatBall = null;
        float maxX = float.MinValue;

        foreach (var ball in balls)
        {
            // Skip this ball if it was destroyed
            if (ball == null)
            {
                continue;
            }

            float ballX = ball.transform.position.x;
            float ballVelX = ball.linearVelocity.x;

           
            if (ballVelX > 0 && ballX > maxX)
            {
                maxX = ballX;
                threatBall = ball;
            }
        }

        
        if (threatBall == null)
        {
            foreach (var ball in balls)
            {

                if (ball == null)
                {
                    continue;
                }

                float ballX = ball.transform.position.x;
                if (ballX > maxX)
                {
                    maxX = ballX;
                    threatBall = ball;
                }
            }
        }

        if (threatBall != null)
        {
            Defense(threatBall.gameObject);
        }
    }

    void Defense(GameObject currentThreat)
    {
        float yDiff = currentThreat.transform.position.y - transform.position.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + (yDiff * Time.deltaTime * moveSpeed), transform.position.z);
    }

}
