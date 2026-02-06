/* For childCount https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-childCount.html
*  This class "observes" the scene counting how many bricks have been broken, and
*  checking if the ball is still in play. It has boolean flags for wins and losses that the
*  PlayLoop checks to see if it's time to reset the game.
*/

using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameObject breakoutBall;
    public GameObject levelGenerator;
    
    // flags for PlayLoop to act on
    public bool sawLoss = false;
    public bool sawWin = false;

    // The number of bricks currently in the scene
    private int numBricks = 0;
    // The number of bricks initially in the scene
    private int initialBricks = 0;
    // The number of bricks that have been broken thus far
    private int bricksBroken = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountBricks();
    }

    // Update is called once per frame
    void Update()
    {
        CountBricks();

        //Check for win or loss
        if (!breakoutBall)
        {
            sawLoss = true;
        }
        else if (numBricks == 0)
        {
            sawWin = true;
        }

    }

    private void CountBricks()
    {
        numBricks = levelGenerator.transform.childCount;
        
        // On the first call, initialBricks needs to be set
        if (initialBricks == 0)
        {
            initialBricks = numBricks;
        }
        else
        {
            bricksBroken = initialBricks - numBricks;
        }
    }

    // This function can be used to calculate a score
    public int getBricksBroken()
    {
        return bricksBroken;
    }
}
