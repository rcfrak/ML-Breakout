// for childCount https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform-childCount.html
using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameObject breakoutBall;
    public GameObject levelGenerator;
    
    public bool sawLoss = false;
    public bool sawWin = false;
    private int numBricks = 0;
    private int initialBricks = 0;
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

    public int getBricksBroken()
    {
        return bricksBroken;
    }
}
