using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private bool leftLost = false;
    private bool rightLost = false;
    public Canvas matchEndCanvas;
    private int numPlayers = 0;

    //This function should be called the first time the play loop is run
    public void addPlayer()
    {
        numPlayers++;
    }
    
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
        if (numPlayers == 1)
        {
            if (leftLost || rightLost)
            {
                gameOver();
            }
        }
        //split screen
        else if (numPlayers == 2)
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
