using UnityEngine;
using TMPro;

public class CoinManager : GameManager
{
    [Header("Coin UI")]
    [SerializeField] private TMP_Text coinText;

    [Header("Coin Spawner (Grid)")]
    [SerializeField] private CoinSpawner coinSpawner; // drag your CoinSpawner here

    private int coinsCollected = 0;
    private int coinTarget = 0;

    protected override void Start()
    {
        // 1) Let the base do its Start (countdown setup, spawner disable, etc)
        base.Start();

        // 2) Initialize coins
        InitializeCoins();
    }

    private void InitializeCoins()
    {
        // Spawn coins once at level start
        if (coinSpawner != null)
        {
            coinSpawner.SpawnCoins();
            
            // Calculate target based on spawner settings
            coinTarget = coinSpawner.columns * coinSpawner.rows;
            
            // DOUBLE CHECK: Count actual coins spawned in the scene
            GameObject[] actualCoins = GameObject.FindGameObjectsWithTag("Coin");
            
            Debug.Log($"=== COIN DEBUG INFO ===");
            Debug.Log($"Expected coins (columns × rows): {coinSpawner.columns} × {coinSpawner.rows} = {coinTarget}");
            Debug.Log($"Actual coins found in scene: {actualCoins.Length}");
            
            // Use the actual count if different
            if (actualCoins.Length != coinTarget)
            {
                Debug.LogWarning($"Mismatch! Expected {coinTarget} but found {actualCoins.Length} coins");
                coinTarget = actualCoins.Length; // Use actual count
            }
            
            coinsCollected = 0; // Reset collection count
            UpdateCoinUI();
            Debug.Log($"Final coin target set to: {coinTarget}");
        }
        else
        {
            Debug.LogError("CoinSpawner not assigned!");
        }
    }

    // Called by each Coin when the Ball triggers it
    public void CollectCoin()
    {
        coinsCollected++;
        
        // Count remaining coins in scene for verification
        GameObject[] remainingCoins = GameObject.FindGameObjectsWithTag("Coin");
        
        Debug.Log($"=== COIN COLLECTED ===");
        Debug.Log($"Coins collected: {coinsCollected}/{coinTarget}");
        Debug.Log($"Coins remaining in scene: {remainingCoins.Length}");
        Debug.Log($"Progress: {(float)coinsCollected / coinTarget * 100f:F1}%");
        Debug.Log($"Expected remaining: {coinTarget - coinsCollected}");
        
        UpdateCoinUI();
        CheckWinCondition();  // re-evaluate with updated coins
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coinsCollected} / {coinTarget}";
        }
        else
        {
            Debug.LogWarning("coinText UI element not assigned!");
        }
    }

    // Override win logic: require either 3 goals OR all coins (not both)
    protected override void CheckWinCondition()
    {
        // Count actual coins in scene for debugging
        GameObject[] currentCoins = GameObject.FindGameObjectsWithTag("Coin");
        
        // Check each condition separately
        bool scoreConditionMet = playerScore >= 3;
        bool coinConditionMet = coinsCollected >= coinTarget;
        bool eitherConditionMet = scoreConditionMet || coinConditionMet;  // OR instead of AND
        
        Debug.Log($"=== WIN CONDITION CHECK ===");
        Debug.Log($"Player Score: {playerScore}/3");
        Debug.Log($"Coins Collected (counter): {coinsCollected}/{coinTarget}");
        Debug.Log($"Coins Still in Scene: {currentCoins.Length}");
        Debug.Log($"Expected coins remaining: {coinTarget - coinsCollected}");
        Debug.Log($"Score condition met: {scoreConditionMet} ({playerScore} >= 3)");
        Debug.Log($"Coin condition met: {coinConditionMet} ({coinsCollected} >= {coinTarget})");
        Debug.Log($"EITHER condition met: {eitherConditionMet} (OR logic)");
        
        // Additional validation
        if (currentCoins.Length != (coinTarget - coinsCollected))
        {
            Debug.LogWarning($"MISMATCH! Expected {coinTarget - coinsCollected} coins in scene, but found {currentCoins.Length}");
        }
        
        if (eitherConditionMet)  // Changed from bothConditionsMet to eitherConditionMet
        {
            Debug.Log("🎉 WIN CONDITION MET! Either score OR coins complete!");
            
            if (scoreConditionMet && coinConditionMet)
            {
                Debug.Log("(Both conditions were actually met!)");
            }
            else if (scoreConditionMet)
            {
                Debug.Log("(Won by scoring 3 goals!)");
            }
            else if (coinConditionMet)
            {
                Debug.Log("(Won by collecting all coins!)");
            }
            
            Debug.Log($"Setting playerWon = true...");
            playerWon = true;
            
            // IMPORTANT: Freeze the ball immediately to stop gameplay
            Debug.Log("Freezing ball to stop gameplay...");
            FreezeBall();
            
            if (audioSource != null && winAudio != null)
            {
                audioSource.PlayOneShot(winAudio);
                Debug.Log("Playing win audio...");
            }
            else
            {
                Debug.LogWarning("audioSource or winAudio is null!");
            }
            
            Debug.Log("Starting Win() coroutine...");
            StartCoroutine(Win());
        }
        else
        {
            Debug.Log($"Win condition NOT met yet - need EITHER {3 - playerScore} more goals OR {coinTarget - coinsCollected} more coins");
        }
    }

    // CRITICAL FIX: Override PlayerScore to prevent automatic ball reset
    public override void PlayerScore()
    {
        Debug.Log("=== PLAYER SCORED ===");
        
        FreezeBall();
        DestroyExtraBalls();
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        
        Debug.Log($"New player score: {playerScore}");
        
        // Check win condition
        CheckWinCondition();

        // IMPORTANT: Only reset if we haven't won
        // This prevents coins from being reset when we might be close to winning
        if (!playerWon)
        {
            audioSource.PlayOneShot(playerScoreAudio);
            
            // DON'T automatically reset ball - let the player continue collecting coins
            // Only reset when they actually fail or when starting a new round
            Debug.Log("Goal scored but win condition not met. Continuing with current coins...");
            
            // Instead of ResetBall(), just restart the ball from center
            RestartBallWithoutResettingCoins();
        }
    }

    // Helper method to restart ball without resetting coin progress
    private void RestartBallWithoutResettingCoins()
    {
        Debug.Log("Restarting ball without resetting coins...");
        
        // Move ball back to center
        ball.transform.position = initialBallPos;
        
        // Re-enable spawner for multi-ball levels
        if (spawner != null) spawner.enabled = false;
        
        // Start countdown to launch ball again
        StartCoroutine(Countdown());
    }

    // Override ResetBall for when we actually need to reset everything (like when player loses or restarts)
    protected override void ResetBall()
    {
        Debug.Log("=== FULL RESET (Ball + Coins) ===");
        
        // 1) Call the base ResetBall first (handles ball positioning, freezing, etc.)
        base.ResetBall();
        
        // 2) Reset coin collection progress
        coinsCollected = 0;
        
        // 3) Destroy any existing coins and respawn them
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("Coin");
        Debug.Log($"Destroying {existingCoins.Length} existing coins");
        foreach (GameObject coin in existingCoins)
        {
            Destroy(coin);
        }
        
        // 4) Respawn all coins
        if (coinSpawner != null)
        {
            coinSpawner.SpawnCoins();
            
            // Recount coins after spawning
            GameObject[] newCoins = GameObject.FindGameObjectsWithTag("Coin");
            coinTarget = newCoins.Length; // Use actual count
            Debug.Log($"Respawned {coinTarget} coins");
        }
        
        // 5) Update UI
        UpdateCoinUI();
        
        Debug.Log($"Full reset complete. Coin target: {coinTarget}");
    }
}