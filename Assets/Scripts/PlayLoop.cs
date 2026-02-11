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
    private Observer observer;
    private Scorer scorer;
    private Button restartButton;
    public UIDocument uiDocument;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        //adds method to list of things done when the button is clicked
        restartButton.clicked += ReloadScene;

        //link to observer and scorer objects in GameManager
        observer = GetComponent<Observer>();
        scorer = GetComponent<Scorer>();
    }

    // Update is called once per frame
    void Update()
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
