// for general script, https://learn.unity.com/course/2d-beginner-game-sprite-flight/tutorial/restart-the-game-with-a-bang?version=6.3

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
