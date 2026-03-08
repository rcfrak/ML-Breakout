/* for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3
 * This class is intended to control the high level flow of the game, serving up  
 * UI and loading/reloading scenes when needed. It utilizes the Scorer and Observer
 * GameManager components to help manage the game. 
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor.Experimental.Rendering;

public class PlayLoop : MonoBehaviour
{
    // Allow for switching functionality based on play or train mode.
    public enum GameMode
    {
        Play,
        Training,
        Inference
    }
    public GameMode mode = GameMode.Play;
    public BreakoutBall ball;
    //public PaddleMovement paddle;
    public PaddleAgent paddle;
    public LevelGenerator levelGenerator;
    private Observer observer;
    private Scorer scorer;
    public Vector2 ballPosition;
    public Vector2 paddlePosition;
    public MatchManager matchManager;
    
    public enum Screen
    {
        Left,
        Right
    }
    public Screen screen;
    private string player;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (screen == Screen.Left)
        {
            player = GameConfig.Instance.getPlayer(1);
        }
        else if (screen == Screen.Right)
        {
            player = GameConfig.Instance.getPlayer(2);
        }

        if (player == "Player") {
            mode = GameMode.Play;
        }
        else
        {
            mode = GameMode.Inference;
            paddle.loadModel(player);
        }

        paddlePosition = paddle.transform.position;
        ballPosition = ball.transform.position;

        //link to observer and scorer objects in GameManager
        observer = GetComponent<Observer>();
        scorer = GetComponent<Scorer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Return to main menu if Escape key is pressed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Clear score when exit to main menu
            ScoreStorage.Instance.ResetAllScore();
            SceneManager.LoadScene("MainMenu");
            return;
        }

        UpdateScoreDisplay();

        // Check if episode is over
        if (!observer.EpisodeOver)
        {
            return;
        }
        
        //The episode is over
        //Handle resetting the loop based on who is playing
        if (mode == GameMode.Play || mode == GameMode.Inference)
        {
            // If player is out of balls, stop giving it more turns
            if (observer.ballsDepleted)
            {
                //wait for the other screen to finish
                return;
            }
            //Else the player won
            if (observer.sawWin)
            {
                scorer.writeWin(screen.ToString());
            }
            ResetPlayerOrInference();
        }
        else if (mode == GameMode.Training)
        {
            // If in training mode, skip restart button and reset episode
            ResetEpisode();
        }
    }

    void ResetPlayerOrInference()
    {
        observer.ResetObserver();
        levelGenerator.ResetLevel();
        paddle.ResetPaddle(paddlePosition);
        ball.ResetBall(ballPosition);
        
        if(mode == GameMode.Inference)
        {
            ball.Launch();
        }
    }

    void ResetEpisode()
    {
        //tell the scorer to write data before reloading the scene
        if (observer.sawWin)
        {
            scorer.writeWin(screen.ToString());
        }
        else if (observer.sawLoss)
        {
            scorer.writeLoss(screen.ToString());
        }
        paddle.EndEpisode();

        // Reset for training episodes
        observer.ResetObserver();
        levelGenerator.ResetLevel();
        paddle.ResetPaddle(paddlePosition);
        ball.ResetBall(ballPosition);
        ball.Launch();
    }

    // Training mode alternative to destroying the ball object on floor impact
    // Reset rather than reload
    public void TriggerLoss()
    {
        observer.sawLoss = true;
        observer.EpisodeOver = true;
    }

    // Register ball loss, check if no balls remain and end game if true
    public void HandleBallLost()
    {
        observer.LoseBall();

        if (observer.ballsDepleted)
        {
            observer.sawLoss = true;
            observer.EpisodeOver = true;
            scorer.writeLoss(screen.ToString());
            matchManager.addLoss(screen.ToString());
            
            ball.ResetBall(ballPosition);
            paddle.ResetPaddle(paddlePosition);

            // hide the ball when the match is over
            ball.gameObject.SetActive(false);
        }
        else
        {
            ball.ResetBall(ballPosition);
            paddle.ResetPaddle(paddlePosition);
            // Need to launch the ball for the model
            if (mode == GameMode.Inference)
            {
                ball.Launch();
            }
        }
    }

    void UpdateScoreDisplay()
    {
        if (scoreDisplay == null)
        {
            return;
        }
        
        int totalScore = scorer.getTotalScore(screen.ToString());
        scoreDisplay.text = totalScore.ToString("00000"); 
    }
}
