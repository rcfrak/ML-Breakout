/* for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3
 * This class is intended to control the high level flow of the game, serving up  
 * UI and loading/reloading scenes when needed. It utilizes the Scorer and Observer
 * GameManager components to help manage the game. 
 */

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    //private Button restartButton;
    //public UIDocument uiDocument;
    public Vector2 ballPosition;
    public Vector2 paddlePosition;
    
    public enum Screen
    {
        Left,
        Right
    }
    public Screen screen;
    private string player;

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
        
        /* This is the first problematic bit of code for this commit
         * We need a restart button to appear when both sides lose
        if (mode == GameMode.Play)
        {
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
            restartButton.style.display = DisplayStyle.None;
            //adds method to list of things done when the button is clicked
            restartButton.clicked += ReloadScene;
        }
        // If game mode is not set to play, hide UI element.
        else
        {
            if (uiDocument != null)
            {
                uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            }
        }
        */

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
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Check if episode is over
        if (!observer.EpisodeOver)
        {
            return;
        }
        
        if (mode == GameMode.Play)
        {
            /*It looks like this code block also needs to be isolated to remove the retry button for now
            // This conditional is split to prepare for different win/loss UIs
            if (observer.sawWin)
            {
                //restartButton.style.display = DisplayStyle.Flex;
            }
            else if (observer.sawLoss)
            {
                //restartButton.style.display = DisplayStyle.Flex;
            }
            */
            // If player is out of balls, stop giving it more turns
            if (observer.ballsDepleted)
            {
                return;
            }

            ResetPlayer();
        }

        // If in training mode, skip restart button and reset episode
        else if (mode == GameMode.Training)
        {
            ResetEpisode();
        }
        else if (mode == GameMode.Inference)
        {
            // If AI is out of balls, stop giving it more turns
            if (observer.ballsDepleted)
            {
                return;
            }

            ResetInference();
        }
    }

    void ResetInference()
    {
        observer.ResetObserver();
        levelGenerator.ResetLevel();
        paddle.ResetPaddle(paddlePosition);
        ball.ResetBall(ballPosition);
        ball.Launch();
    }

    void ResetPlayer()
    {
        observer.ResetObserver();
        levelGenerator.ResetLevel();
        paddle.ResetPaddle(paddlePosition);
        ball.ResetBall(ballPosition);
    }

    void ResetEpisode()
    {
        //tell the scorer to write data before reloading the scene
        if (observer.sawWin)
        {
            scorer.writeWin();
        }
        else if (observer.sawLoss)
        {
            scorer.writeLoss();
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

    //Reload the scene by loading the scene with current scene name
    //See referenced tutorial step 9.1
    void ReloadScene()
    {
        //tell the scorer to write data before reloading the scene
        if (observer.sawWin)
        {
            scorer.writeWin();
        }
        else if (observer.sawLoss)
        {
            scorer.writeLoss();
        }

        observer.FullReset();

        //Now reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Register ball loss, check if no balls remain and end game if true
    public void HandleBallLost()
    {
        observer.LoseBall();

        if (observer.ballsDepleted)
        {
            observer.sawLoss = true;
            observer.EpisodeOver = true;

            if (mode == GameMode.Play)
            {
                Destroy(ball.gameObject);
            }
            else if (mode == GameMode.Inference)
            {
                ball.ResetBall(ballPosition);
                paddle.ResetPaddle(paddlePosition);
            }
        }
        else
        {
            ball.ResetBall(ballPosition);
            paddle.ResetPaddle(paddlePosition);

            if (mode == GameMode.Inference)
            {
                ball.Launch();
            }
        }
    }
}
