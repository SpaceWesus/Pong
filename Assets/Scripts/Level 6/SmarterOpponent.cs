using UnityEngine;
using System.Linq;

public class OpponentController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY =  4f;

    void Update()
    {
        // 1. Grab all balls
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        if (balls.Length == 0)
            return;

        // 2. Pick the ball whose center-to-paddle distance is smallest
        Transform target = balls
            .OrderBy(b => Vector2.Distance(b.transform.position, transform.position))
            .First()
            .transform;

        // 3. Move straight toward its Y
        float y = Mathf.Clamp(target.position.y, minY, maxY);
        Vector3 goal = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = Vector3.MoveTowards(
            transform.position,
            goal,
            speed * Time.deltaTime
        );
    }
}


