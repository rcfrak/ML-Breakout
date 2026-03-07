using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(changeScene);  
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(changeScene);
    }

    private void changeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
