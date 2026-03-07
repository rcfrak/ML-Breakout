using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton;
    void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
    }

    void OnDestroy()
    {
        exitButton.onClick.RemoveListener(ExitGame);
    }

    void ExitGame()
    {
        Application.Quit();
        // preprocessor directive to check if using Unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
