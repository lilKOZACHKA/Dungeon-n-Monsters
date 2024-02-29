using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private const double V = 1.03;
    private Rigidbody2D rb;
    public float normalSpeed = 2f;
    public float sprintSpeed = 4f;
    public float size_x = (float)V;
    private Vector2 moveVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");
        moveVector.Normalize();

        // Check for sprinting (holding down the Shift key)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

        // Apply speed to the movement vector
        moveVector *= currentSpeed;

        // Flip the sprite if moving to the left
        if (moveVector.x < 0)
        {
            FlipSprite(true);
        }
        else if (moveVector.x > 0)
        {
            FlipSprite(false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
    }

    void FlipSprite(bool facingLeft)
    {
        // Multiply the local scale by -1 on the X-axis to flip the sprite
        Vector3 newScale = transform.localScale;
        newScale.x = facingLeft ? -size_x : size_x;
        transform.localScale = newScale;
    }
}
