// for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayLoop : MonoBehaviour
{
    private Observer observer;
    private Button restartButton;
    public UIDocument uiDocument;

    public const int scoreMultiplier = 10;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        //adds method to list of things done when the button is clicked
        restartButton.clicked += ReloadScene;

        //link to observer object in GameManager
        observer = GetComponent<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        score = observer.getBricksBroken() * scoreMultiplier;

        if (observer.sawWin)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }
        else if (observer.sawLoss)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }

        Debug.Log(score + ScoreManager.Instance.getSavedScore());
    }

    //Reload the scene by loading the scene with current scene name
    //See referenced tutorial step 9.1
    void ReloadScene()
    {
        //Save the score by adding it to the cumulative score
        ScoreManager.Instance.addScore(score);

        if (observer.sawWin)
        {
            ScoreManager.Instance.addWin();
        }
        else if (observer.sawLoss)
        {
            ScoreManager.Instance.gameLost();
        }

        //Now reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
