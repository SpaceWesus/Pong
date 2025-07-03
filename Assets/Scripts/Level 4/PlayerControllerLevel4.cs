using UnityEngine;

public class PlayerControllerLevel4 : PlayerController
{
    [SerializeField] private float spinSpeed = 100f;
    [SerializeField] private float spinDirection = 1f;

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1)) spinDirection = spinDirection * -1;

        // Update the player paddle's position to match the mouse's y position (within specified bounds).
        float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        if (yPos < minYPos) yPos = minYPos;
        if (yPos > maxYPos) yPos = maxYPos;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (spinSpeed * Time.deltaTime * spinDirection));
    }
}
