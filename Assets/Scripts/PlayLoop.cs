/* for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3
 * This class is intended to control the high level flow of the game, serving up  
 * UI and loading/reloading scenes when needed. It utilizes the Scorer and Observer
 * GameManager components to help manage the game. 
 */

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayLoop : MonoBehaviour
{
    // Allow for switching functionality based on play or train mode.
    public enum GameMode
    {
        Play,
        Training
    }
    public GameMode mode = GameMode.Play;
    public BreakoutBall ball;
    public PaddleMovement paddle;
    public LevelGenerator levelGenerator;
    private Observer observer;
    private Scorer scorer;
    private Button restartButton;
    public UIDocument uiDocument;
    private Vector2 ballPosition;
    private Vector2 paddlePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paddlePosition = paddle.transform.position;
        ballPosition = ball.transform.position;
        
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

        //link to observer and scorer objects in GameManager
        observer = GetComponent<Observer>();
        scorer = GetComponent<Scorer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!observer.EpisodeOver)
        {
            return;
        }
        
        if (mode == GameMode.Play)
        {
            // This conditional is split to prepare for different win/loss UIs
            if (observer.sawWin)
            {
                restartButton.style.display = DisplayStyle.Flex;
            }
            else if (observer.sawLoss)
            {
                restartButton.style.display = DisplayStyle.Flex;
            } 
        }
        else
        {
            ResetEpisode();
        }
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

        // Reset rather than reload for training episodes
        observer.ResetObserver();
        levelGenerator.ResetLevel();
        paddle.ResetPaddle(paddlePosition);
        ball.ResetBall(ballPosition);
        ball.Launch();
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

        //Now reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
