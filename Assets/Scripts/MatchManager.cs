using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    private bool leftLost = false;
    private bool rightLost = false;
    public Canvas matchEndCanvas;
    
    //This function should be called by each game play loop when their side is out of lives
    public void addLoss(string side)
    {
        if (side == "Left")
        {
            leftLost = true;
        }
        else if (side == "Right")
        {
            rightLost = true;
        }
        else
        {
            Debug.Log("The match manager is being used incorrectly with side named: " + side);
        }

        //solo mode
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (leftLost || rightLost)
            {
                gameOver();
            }
        }
        //split screen
        else if (SceneManager.GetActiveScene().name == "SplitScreen")
        {
            if (leftLost && rightLost)
            {
                gameOver();
            }
        }
        
    }
    void gameOver()
    {
        matchEndCanvas.gameObject.SetActive(true);
    }
}
