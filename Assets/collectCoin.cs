// ===== COIN.CS =====
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Coin : MonoBehaviour
{
    private void Awake()
    {
        // 1) Ensure the collider is a trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // 2) Ensure there's a kinematic Rigidbody2D
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType     = RigidbodyType2D.Kinematic;
        rb.simulated    = true;
        rb.useFullKinematicContacts = true;  // optional: improves trigger reliability
        rb.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Coin] Triggered by {other.name} (tag={other.tag})");

        if (!other.CompareTag("Ball"))
        {
            Debug.Log("[Coin] Ignored, not a Ball.");
            return;
        }

        var cm = FindObjectOfType<CoinManager>();
        if (cm != null)
        {
            cm.CollectCoin();
            Debug.Log("[Coin] Collected!");
        }
        else
        {
            Debug.LogError("[Coin] CoinManager not found!");
        }

        Destroy(gameObject);
    }
}


