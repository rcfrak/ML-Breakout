using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField] private PlayLoop playLoop;
    public InputAction MoveAction;
    private Rigidbody2D rigid_body;

    public float speed = 8.00f;
    private float left_boundary;
    private float right_boundary;
    private float half_paddle;
    private float input;

    // Experimenting with Rigidbody2D to help smooth physics and movement stuttering.
    void Awake()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();

        // Locate the game objects in hierarchy
        GameObject left_wall = GameObject.Find("Left_Wall");
        GameObject right_wall = GameObject.Find("Right_Wall");
        GameObject paddle = GameObject.Find("Paddle");

        // Get the properties of the sprite to access the bounds
        Bounds left_bounds = left_wall.GetComponent<SpriteRenderer>().bounds;
        Bounds right_bounds = right_wall.GetComponent<SpriteRenderer>().bounds;
        Bounds paddle_bounds = paddle.GetComponent<SpriteRenderer>().bounds;

        // Set paddle boundaries based on the left and right walls, taking into account the paddle size
        half_paddle = paddle_bounds.extents.x;
        left_boundary = left_bounds.max.x + half_paddle;
        right_boundary = right_bounds.min.x - half_paddle;
    }

    // Switched from Update in an attempt to smooth the physics and movement stuttering.
    // Seems slightly better on my PC. Not sure if it is a major improvement.
    void FixedUpdate()
    {
        if (playLoop.mode != PlayLoop.GameMode.Play)
        {
            return;
        }

        input = MoveAction.ReadValue<Vector2>().x;

        Vector2 target = rigid_body.position;
        target.x += input * speed * Time.fixedDeltaTime;
        target.x = Mathf.Clamp(target.x, left_boundary, right_boundary);

        rigid_body.MovePosition(target);
    }

    public void ResetPaddle(Vector2 startPosition)
    {
        rigid_body.position = startPosition;
    }
}
