using UnityEngine;
using UnityEngine.UI;

public class SoloButton : MonoBehaviour
{
    public Button button;
    public Canvas soloCanvas;
    public Canvas mainMenuCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(changeMenu);  
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(changeMenu);
    }

    private void changeMenu()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        soloCanvas.gameObject.SetActive(true);
    }
}
