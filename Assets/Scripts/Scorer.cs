/* This class calculates a score for each round based on the number of bricks 
 * broken thus far. The scorer communicates with ScoreStorage to write scores
 * at the end of a round (win or loss), and get a cumulative score
 */

using System.Diagnostics;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    private Observer observer;

    public const int scoreMultiplier = 10;
    private int score = 0;
    private int paddleHitCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //link to observer object in GameManager
        observer = GetComponent<Observer>();
    }

    // Update is called once per frame
    void Update()
    {
        score = observer.getBricksBroken() * scoreMultiplier;
        paddleHitCount = observer.getPaddleHits();
        // Debug.Log($"Paddle hits: {paddleHitCount}");
        UnityEngine.Debug.Log($"Left Score: {getTotalScore("Left")}");
        // Debug.Log($"Rounds Won: {ScoreStorage.Instance.getRoundsWon()}");
        //Debug.Log(getTotalScore());
        //Debug.Log($"Cumulative Reward: {GetCumulativeReward()}");
    }

    public void writeWin(string side)
    {
        ScoreStorage.Instance.addScore(side, score);
        ScoreStorage.Instance.addWin(side);
    }

    public void writeLoss(string side)
    {
        ScoreStorage.Instance.addScore(side, score);
        ScoreStorage.Instance.gameLost(side);
    }

    public int getTotalScore(string side)
    {
        return score + ScoreStorage.Instance.getSavedScore(side);
    }

    

}
