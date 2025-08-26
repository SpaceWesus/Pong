using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBall : MonoBehaviour
{
    [Header("Bounce & Wind Settings")]
    public float paddleBoost = 4f;
    public float windStrength = 0f;

    [Header("Gravity Flip Timing (secs)")]
    [Tooltip("Next flip is chosen at random between these two values")]
    public float minFlipInterval = 8f;
    public float maxFlipInterval = 15f;

    Rigidbody2D rb;
    float flipTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ScheduleNextFlip();
    }

    void Update()
    {
        // Countdown to next gravity flip
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0f)
        {
            FlipGravity();
            ScheduleNextFlip();
        }
    }

    void FixedUpdate()
    {
        // Optional random “wind” nudges
        if (windStrength > 0f)
            rb.linearVelocity += Random.insideUnitCircle.normalized * windStrength * Time.fixedDeltaTime;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.CompareTag("Paddle"))
            rb.linearVelocity += Vector2.up * paddleBoost;
    }

    void FlipGravity()
    {
        // invert the global 2D gravity
        Physics2D.gravity = -Physics2D.gravity;
    }

    void ScheduleNextFlip()
    {
        flipTimer = Random.Range(minFlipInterval, maxFlipInterval);
    }
}
