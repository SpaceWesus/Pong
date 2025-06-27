using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class Opponent : MonoBehaviour
{

    [SerializeField] protected GameObject ball;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.position.y > rb.transform.position.y)
        {
            rb.AddForce(Vector2.up);
        } else {rb.AddForce(Vector2.down);}
    }
}
