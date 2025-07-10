using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    public float wrapYPosition;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        Vector2 pos = rb.position;
        pos.y = wrapYPosition;
        rb.position = pos;

        // Invert Y velocity to counteract Unity's automatic bounce
        Vector2 vel = rb.linearVelocity;
        vel.y = -vel.y;
        rb.linearVelocity = vel;
    }

}
