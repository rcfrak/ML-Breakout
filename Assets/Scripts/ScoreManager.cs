/* This script is adapted from https://gist.github.com/kurtdekker/775bb97614047072f7004d6fb9ccce30
 * to prepare a game manager singleton - a class instantiation that can only
 * only exist once - and mark it with DontDestroyOnLoad().
 * The game manager keeps track of the player's score between scene reloads, 
 * preserving their score if they clear a level.
 *
 * To access this object from another script, use ScoreManager.Instance and
 * do not add it to a game scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _Instance;
    public static ScoreManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<ScoreManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private int savedScore = 0;
    private int roundsWon = 0;

    // Call this function to add a score after a win or loss
    public void addScore(int score)
    {
        savedScore += score;
    }

    // Increment the number of rounds won in a play session
    public void addWin()
    {
        roundsWon++;
    }

    // Reset the cumulative score and number of wins when the ball is lost
    public void gameLost()
    {
        savedScore = 0;
        roundsWon = 0;
    }

    // Returns the cumulative score obtained from prior, sequential wins
    public int getSavedScore()
    {
        return savedScore;
    }
}

