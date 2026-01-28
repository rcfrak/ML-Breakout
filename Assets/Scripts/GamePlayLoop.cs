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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

        numBricks = levelGenerator.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        numBricks = levelGenerator.transform.childCount;

        if (breakoutBall == null)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }

        if (numBricks == 0)
        {
            restartButton.style.display = DisplayStyle.Flex;
        }
    }
 

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
