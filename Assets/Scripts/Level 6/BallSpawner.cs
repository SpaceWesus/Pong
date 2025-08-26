using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Prefab & Caps")]
    [Tooltip("Your Ball prefab (must be tagged 'Ball')")]
    public GameObject ballPrefab;

    [Tooltip("Max balls on screen at once")]
    public int maxConcurrentBalls = 3;

    [Header("Timing")]
    [Tooltip("Seconds before first extra spawns")]
    public float initialSpawnDelay = 5f;
    [Tooltip("Seconds between each spawn attempt")]
    public float spawnInterval = 8f;

    [Header("Speed Settings")]
    [Tooltip("Base speed applied to every ball")]
    public float baseBallSpeed = 6f;
    [Tooltip("Random multiplier range")]
    public float minSpeedMultiplier = 0.8f;
    public float maxSpeedMultiplier = 1.2f;

    private float timer;

    void Start()
    {
        timer = initialSpawnDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // count how many balls are alive right now
            int current = GameObject.FindGameObjectsWithTag("Ball").Length;

            if (current < maxConcurrentBalls)
                SpawnBall();

            timer = spawnInterval;
        }
    }

    void SpawnBall()
    {
        // instantiate at center
        GameObject go = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

        // pick a random direction
        Vector2 dir = Random.insideUnitCircle.normalized;

        // pick a random speed factor
        float factor = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);

        // assign velocity
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = dir * baseBallSpeed * factor;
    }
}

