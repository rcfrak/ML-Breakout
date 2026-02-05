// for childCount https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-childCount.html
// for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GamePlayLoop : MonoBehaviour
{
    private Button restartButton;
    public GameObject breakoutBall;
    public GameObject levelGenerator;
    public UIDocument uiDocument;

    private int numBricks = 0;
    public const int scoreMultiplier = 10;
    private int initialBricks = 0;
    private int bricksBroken = 0;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        //adds method to list of things done when the button is clicked
        restartButton.clicked += ReloadScene;

        numBricks = levelGenerator.transform.childCount;
        bricksBroken = 0;
        initialBricks = numBricks;
    }

    // Update is called once per frame
    void Update()
    {
        //count the number of brick clones remaining hierarchically under the levelGenerator
        numBricks = levelGenerator.transform.childCount;
        bricksBroken = initialBricks - numBricks;
        score = bricksBroken * scoreMultiplier;
        Debug.Log(score + GameManager.Instance.getSavedScore());

        if (breakoutBall == null || numBricks == 0)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }
    }
 
    //Reload the scene by loading the scene with current scene name
    //See referenced tutorial step 9.1
    void ReloadScene()
    {
        GameManager.Instance.addScore(score);
        if (numBricks == 0)
        {
            GameManager.Instance.addWin();
        }
        else
        {
            GameManager.Instance.gameLost();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
