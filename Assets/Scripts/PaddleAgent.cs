using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using System.Diagnostics;


[RequireComponent(typeof(Rigidbody2D))]
public class PaddleAgent : Agent
{
    [Header("References")]
    [SerializeField] private PlayLoop playLoop;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private BreakoutBall breakoutBall;

    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float leftBoundary = -7.5f;
    [SerializeField] private float rightBoundary = 7.5f;
    [SerializeField] private float topBoundary = 9.5f;
    [SerializeField] private float bottomBoundary = -9.5f;

    [Header("Difficulty")]
    [SerializeField] private int playerIndex = 2;

    [Header("Ball (Observations)")]
    [SerializeField] private Transform ballTransform;
    [SerializeField] private Rigidbody2D ballRb;

    private Rigidbody2D rb;
    private float inputX;

    private static readonly int[] DecisionPeriods = { 6, 3, 1 };
    private static readonly float[] PaddleScales = { 1.2f, 1f, 1f };
    private static readonly float[] BallSpeeds = { 8f, 10f, 14f };

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        if (moveAction != null)
            moveAction.action.Enable();

        ApplyDifficulty();
    }

    private void ApplyDifficulty()
    {
        string difficulty = GameConfig.Instance.getPlayer(playerIndex);

        int diffIndex = difficulty switch
        {
            "Easy" => 0,
            "Medium" => 1,
            "Hard" => 2,

            //default to medium if some error occurs and nothing is picked or if they launch splitscreen and don't go thorugh the
            // menu
            _ => 1
        };

        // set new decision periods using difficulty index
        var dr = GetComponent<DecisionRequester>();
        if (dr != null)
            dr.DecisionPeriod = DecisionPeriods[diffIndex];

        //set new paddle size using difficulty index
        Vector3 s = transform.localScale;
        transform.localScale = new Vector3(PaddleScales[diffIndex] * s.x, s.y, s.z);

        //set new ball speed using difficulty index
        if (breakoutBall != null)
            breakoutBall.ballSpeed = BallSpeeds[diffIndex];

        UnityEngine.Debug.Log($"[PaddleAgent P{playerIndex}] Difficulty: {difficulty}");
    }

    void FixedUpdate()
    {
        // HUMAN CONTROL
        if (playLoop.mode == PlayLoop.GameMode.Play)
            inputX = moveAction.action.ReadValue<Vector2>().x;
        // TRAINING: inputX already set by Agent

        MovePaddle();

        // Reward for being close to the ball
        // Attempting to help track better
        float disX = Mathf.Abs(transform.position.x - ballTransform.position.x);
        float proxReward = Mathf.Clamp01(1f - disX / (rightBoundary - leftBoundary));

        AddReward(0.005f * proxReward);

        //Reward to move more since paddle started to sit in the middle
        AddReward(0.001f);

        if (Time.frameCount % 120 == 0)
        {
            var dr = GetComponent<DecisionRequester>();
            UnityEngine.Debug.Log($"[PaddleAgent P{playerIndex}] Difficulty: {GameConfig.Instance.getPlayer(playerIndex)}");
        }
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
        // clears agent input so it doesn't drift
        inputX = 0f;
    }


    // ================= ML AGENT =================

    public override void CollectObservations(VectorSensor sensor)
    {
        // Obersation of paddle position
        sensor.AddObservation(NormX(transform.position.x));


        if (ballTransform != null)
        {
            // Observations of the ball X and Y position, as well as direction relative to paddle
            sensor.AddObservation(NormX(ballTransform.position.x));
            sensor.AddObservation(NormY(ballTransform.position.y));
            sensor.AddObservation(NormX(ballTransform.position.x) - NormX(transform.position.x));
            //Debug.Log($"Paddle: {NormX(transform.position.x)} Ball: ({NormX(ballTransform.position.x)}, {ballTransform.position.y})");
        }
        if (ballRb != null)
        {
            // Observation of ball velocity
            Vector2 v = ballRb.linearVelocity.normalized;
            sensor.AddObservation(v.x);
            sensor.AddObservation(v.y);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (playLoop.mode != PlayLoop.GameMode.Training && playLoop.mode != PlayLoop.GameMode.Inference)
            return;

        int action = actions.DiscreteActions[0];
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
    float NormY(float y)
    {
        return Mathf.InverseLerp(bottomBoundary, topBoundary, y) * 2f - 1f;
    }
}

