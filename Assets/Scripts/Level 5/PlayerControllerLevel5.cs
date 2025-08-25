using UnityEngine;

public class PlayerControllerLevel5 : PlayerController
{
    [SerializeField] protected float minXPos;
    [SerializeField] protected float maxXPos;

    // Update is called once per frame
    protected override void Update()
    {
        // Update the player paddle's position to match the mouse's y position (within specified bounds).
        float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float xPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (yPos < minYPos) yPos = minYPos;
        if (yPos > maxYPos) yPos = maxYPos;
        if (xPos < minXPos) xPos = minXPos;
        if (xPos > maxXPos) xPos = maxXPos;
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
