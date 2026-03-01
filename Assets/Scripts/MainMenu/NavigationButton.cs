using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public Button button;
    public Canvas targetCanvas;
    public Canvas thisCanvas;

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
        thisCanvas.gameObject.SetActive(false);
        targetCanvas.gameObject.SetActive(true);
    }
}
