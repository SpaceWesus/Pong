using UnityEngine;


public class BallLevel4 : Ball
{
    [Header("Spinning")]
    [SerializeField] private float spinSpeed;
    [SerializeField] private float spinDirection;

    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (spinSpeed * Time.deltaTime * spinDirection));
    }
}
