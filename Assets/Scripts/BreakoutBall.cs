using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class BreakoutBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    public float ballSpeed = 12f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;   // ensures ball does not fall at start
        rb.linearVelocity = Vector2.zero;   // ball starts still
    }

    void Update()
    {
        // launch towards paddle only after player presses the space bar
        if (hasFallen) return;
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.down * ballSpeed;   
            hasFallen = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // FOR NOW - only react to the paddle
        if (!collision.collider.CompareTag("Paddle")) return;

        // contact point 
        ContactPoint2D cp = collision.GetContact(0);
        Vector2 contactPoint = cp.point;     // world-space contact position
        Vector2 normal = cp.normal;   // collision normal (direction of the surface)
        Vector2 paddleCenter = collision.collider.bounds.center;
        float offsetX = contactPoint.x - paddleCenter.x;    // 0 -> hit center of paddle, > 0 value -> hit right side of paddle
        float halfWidthPaddle = collision.collider.bounds.extents.x; // calculate the paddle_width/2 to normalize offset
        float normalizedXOffset = offsetX / halfWidthPaddle;

        Vector2 newDirection = new Vector2(normalizedXOffset, 1f).normalized;
        rb.linearVelocity = newDirection * ballSpeed;




        Debug.Log($"Hit paddle at {contactPoint}, normal {normal}");
        //Debug.Log($"The paddle's center is at {paddleCenter}");
        Debug.Log($"XOffset relative to padde {normalizedXOffset}");
    }
}
