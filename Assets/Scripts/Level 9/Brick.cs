using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private bool hit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hit) return;
        gameManager.PlayerScore();
        hit = true;
        StartCoroutine(DestroyBrick());
    }

    private IEnumerator DestroyBrick()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}
