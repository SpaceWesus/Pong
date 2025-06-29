using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Bounds")]
    [SerializeField] protected float minYPos;
    [SerializeField] protected float maxYPos;

    protected virtual void Start()
    {
        // Hide the cursor when playing
        Cursor.visible = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Update the player paddle's position to match the mouse's y position (within specified bounds).
        float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        if (yPos < minYPos) yPos = minYPos;
        if (yPos > maxYPos) yPos = maxYPos;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
