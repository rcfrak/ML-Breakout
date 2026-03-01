using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplitStartButton : MonoBehaviour
{
    public Button button;
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;

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
        int index1 = dropdown1.value;
        string selection1 = dropdown1.options[index1].text;
        int index2 = dropdown2.value;
        string selection2 = dropdown2.options[index2].text;

        //Report to the configuration mananger
        GameConfig.Instance.setPlayer(1, selection1);
        GameConfig.Instance.setPlayer(2, selection2);

        //Load the game scene (THIS SHOULD BE UPDATED WHEN THE SECOND SCENE IS CREATED)
        SceneManager.LoadScene("GameScene");
    }
}
