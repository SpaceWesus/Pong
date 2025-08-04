using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Tooltip("Animated Coin prefab (with Coin.cs)")]
    public GameObject coinPrefab;

    [Tooltip("How many columns of coins")]
    public int columns = 5;

    [Tooltip("How many rows of coins")]
    public int rows = 2;

    [Tooltip("Spacing between coins on X (horizontal) and Y (vertical)")]
    public Vector2 spacing = new Vector2(2f, 2f);

    // REMOVED the Start() method that was auto-spawning!
    // Now CoinManager has full control over when to spawn

    /// <summary>
    /// Clears any existing Coin instances in the scene and lays out a columns×rows grid.
    /// </summary>
    public void SpawnCoins()
    {
        Debug.Log($"[CoinSpawner] SpawnCoins called - spawning {columns}x{rows} = {columns * rows} coins");
        
        // 1) Destroy all old coins
        GameObject[] oldCoins = GameObject.FindGameObjectsWithTag("Coin");
        Debug.Log($"[CoinSpawner] Destroying {oldCoins.Length} existing coins");
        foreach (var c in FindObjectsOfType<Coin>())
            Destroy(c.gameObject);

        // 2) Figure out where to start so the grid is centered
        Vector2 origin = (Vector2)transform.position
                       - new Vector2((columns - 1) * spacing.x / 2f,
                                     (rows    - 1) * spacing.y / 2f);

        // 3) Instantiate the grid
        int spawnedCount = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 pos = origin + new Vector2(x * spacing.x, y * spacing.y);
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                spawnedCount++;
                Debug.Log($"[CoinSpawner] Spawned coin {spawnedCount} at {pos}");
            }
        }
        
        Debug.Log($"[CoinSpawner] Finished spawning {spawnedCount} coins");
    }

    // Visualize the spawn area in the Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 size = new Vector2(
            (columns - 1) * spacing.x + 1f,
            (rows - 1) * spacing.y + 1f
        );
        Gizmos.DrawWireCube(transform.position, size);
    }
}