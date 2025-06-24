using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float minYPos;
    [SerializeField] private float maxYPos;

    void Start()
    {
        // Hide the cursor when playing
        Cursor.visible = false;

        //TODO: reenable cursor visibility after the match is over??? 
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        if (yPos < minYPos) yPos = minYPos;
        if (yPos > maxYPos) yPos = maxYPos;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
