using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] protected float minX, maxX;
    [SerializeField] protected float minY, MaxY;

    private float randomX;
    private float randomY;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Add a way to make sure the ball stays within a certain DEGREE of variance from 
        // the direction its expected to go and within a certain SPEED minimum and maximum.

        // Generate random values between min and max 
        randomX = Random.Range(minX, maxX);
        randomY = Random.Range(minY, MaxY);

        // Add random force to the ball 
        rb.AddForce(new Vector2(randomX, randomY), ForceMode2D.Force);
    }
}
