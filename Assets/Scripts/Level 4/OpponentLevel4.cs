using UnityEngine;

public class OpponentLevel4 : Opponent
{
    // Update is called once per frame
    protected override void Update()
    {
        Vector2 toBall = ball.transform.position - transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + (toBall.y * Time.deltaTime * moveSpeed), transform.position.z);

        float angleToBall = Mathf.Clamp(Mathf.Atan(toBall.y / toBall.x) * (180f / Mathf.PI), -30, 30);
        transform.eulerAngles = new Vector3(0f, 0f, angleToBall * -1);
    }
}
