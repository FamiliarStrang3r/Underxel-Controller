using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    private Rigidbody2D rb = null;
    private Vector3 startPosition = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        rb.velocity = Vector3.left * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var wall = collision.gameObject.GetComponent<Wall>();

        if (wall)
        {
            transform.position = startPosition;
            rb.velocity = Vector2.left * speed;
        }
        else
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 direction = Vector2.Reflect(rb.velocity, normal).normalized;
            rb.velocity = direction * speed;

            if (collision.gameObject.CompareTag("EditorOnly"))
            {
                Debug.Log(normal);
            }
        }
    }
}
