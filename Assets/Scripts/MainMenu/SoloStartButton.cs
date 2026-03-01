using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SoloStartButton : MonoBehaviour
{
    public Button button;
    public TMP_Dropdown dropdown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        button.onClick.RemoveListener(startGame);
    }
    void startGame()
    {
        //Read the dropdowns
        int index = dropdown.value;
        string selection = dropdown.options[index].text;

        //Report to the configuration mananger
        GameConfig.Instance.setPlayer(1, selection);

        //Load the game scene
        SceneManager.LoadScene("GameScene");
    }
}
