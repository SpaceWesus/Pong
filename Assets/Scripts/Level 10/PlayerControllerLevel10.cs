using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using TMPro;


public class PlayerControllerLevel10 : PlayerController
{

    [Header("Level 10 References")]
    [SerializeField] public GameObject ActiveBalls;
    [SerializeField] protected GameObject ballPrefab;

    // create a reference to the currentBallCount UI text element 
    [SerializeField] protected TMP_Text currentBallCountText;

    // create a reference to a negativeSoundEffect
    [SerializeField] protected AudioClip CantShootAudio;
    private AudioSource audioSource;
    [SerializeField] public int maxNumOfBalls;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        //currentBallCountText.text = "ActiveBalls: 0";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Shoot();
    }

    public void Shoot()
    {
        Rigidbody2D[] balls = ActiveBalls.GetComponentsInChildren<Rigidbody2D>();

        if (Input.GetMouseButtonDown(0) && balls.Length < maxNumOfBalls)
        {
            // 1. Instantiate the ball at the paddle's position (offset to the right)
            Vector3 spawnPos = transform.position + Vector3.right * 2f;
            GameObject newBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity, ActiveBalls.transform);

            // 2. Get the aiming direction (replace this with your reticle's direction if you have one)
            // Example: oscillate angle between -20 and +20 degrees
            float angle = Mathf.Sin(Time.time * 2f) * 20f; // oscillates between -20 and +20
            Vector2 shootDir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            // 3. Add force to the ball
            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            float shootForce = 10f; // Adjust as needed
            rb.AddForce(shootDir.normalized * shootForce, ForceMode2D.Impulse);

            // Update UI
            currentBallCountText.text = "Active Balls: " + (balls.Length + 1).ToString();
        }
        else CantShoot();
    }

    private void CantShoot()
    {
        // play negative SFX
        audioSource.PlayOneShot(CantShootAudio);

        // Change currentBallCount UI text element color to red
        currentBallCountText.color = Color.red;
        // Shake currentBallCount UI text element
        StartCoroutine(ShakeText(currentBallCountText, 0.2f, 10f));
        // Change currentBallCount UI text element color back to white 
        StartCoroutine(ResetTextColor(currentBallCountText, 0.2f));

    }
    private IEnumerator ResetTextColor(TMP_Text text, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        text.color = Color.white;
    }

    private IEnumerator ShakeText(TMP_Text text, float duration = 0.2f, float magnitude = 10f)
    {
        Vector3 originalPos = text.rectTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            text.rectTransform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        text.rectTransform.localPosition = originalPos;
    }

}
