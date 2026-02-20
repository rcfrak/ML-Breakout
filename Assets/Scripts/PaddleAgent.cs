using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleAgent : Agent
{
    [Header("References")]
    [SerializeField] private PlayLoop playLoop;
    [SerializeField] private InputActionReference moveAction;

    [Header("Movement")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float leftBoundary = -7.5f;
    [SerializeField] private float rightBoundary = 7.5f;

    [Header("Ball (Observations)")]
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Rigidbody2D ballRb;

    private Rigidbody2D rb;
    private float inputX;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();

        if (moveAction != null)
            moveAction.action.Enable();
    }

    void FixedUpdate()
    {
        // HUMAN CONTROL
        if (playLoop.mode == PlayLoop.GameMode.Play)
        {
            inputX = moveAction.action.ReadValue<Vector2>().x;
        }
        // TRAINING: inputX already set by Agent

        MovePaddle();
    }

    void MovePaddle()
    {
        Vector2 target = rb.position;
        target.x += inputX * speed * Time.fixedDeltaTime;
        target.x = Mathf.Clamp(target.x, leftBoundary, rightBoundary);

        rb.MovePosition(target);
    }
    public void ResetPaddle(Vector2 startPosition)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        // clears agent input so it doesn't drift
        inputX = 0f;
    }

    // ================= ML AGENT =================

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(NormX(transform.position.x));

        if (ballTransform != null)
        {
            sensor.AddObservation(NormX(ballTransform.position.x));
            sensor.AddObservation(ballTransform.position.y);
        }

        if (ballRb != null)
        {
            sensor.AddObservation(ballRb.linearVelocity.x);
            sensor.AddObservation(ballRb.linearVelocity.y);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (playLoop.mode != PlayLoop.GameMode.Training)
            return;

        int action = actions.DiscreteActions[0];
        // Debug.Log($"Action = {action}");

        inputX = 0f;
        if (action == 1) inputX = -1f;
        if (action == 2) inputX = 1f;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var d = actionsOut.DiscreteActions;
        d[0] = 0;

        if (Keyboard.current.leftArrowKey.isPressed) d[0] = 1;
        if (Keyboard.current.rightArrowKey.isPressed) d[0] = 2;
    }

    float NormX(float x)
    {
        return Mathf.InverseLerp(leftBoundary, rightBoundary, x) * 2f - 1f;
    }
}