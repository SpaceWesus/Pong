using UnityEngine;


public class Ball : MonoBehaviour
{
    [Header("Angle Nudge on Bounce")]
    [SerializeField] protected float minAngleNudge;
    [SerializeField] protected float maxAngleNudge;

    [Header("Multiply Velocity on Bounce")]
    [SerializeField] protected float bounceVelocityMultiplier;

    [Header("Audio")]
    [SerializeField] protected AudioClip bounceClip;
    protected AudioSource audioSource;

    protected Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // When the ball collides with something, the resulting movement direction is nudged by some random amount.
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Nudge the ball's movement direction a little upon impact.
        float thetaDegrees = Random.Range(minAngleNudge, maxAngleNudge);
        float thetaRadians = thetaDegrees * (Mathf.PI / 180);
        rb.linearVelocity = new Vector2((rb.linearVelocity.x * Mathf.Cos(thetaRadians)) - (rb.linearVelocity.y * Mathf.Sin(thetaRadians)), 
                                        (rb.linearVelocity.x * Mathf.Sin(thetaRadians)) + (rb.linearVelocity.y * Mathf.Cos(thetaRadians)));

        // Multiply the ball's velocity upon impact.
        rb.linearVelocity *= bounceVelocityMultiplier;

        // Play the bounce audio as long as the ball isn't bouncing off of one of the goals.
        if (!collision.gameObject.CompareTag("Player Goal") && !collision.gameObject.CompareTag("Opponent Goal")) audioSource.PlayOneShot(bounceClip);
    }
}
