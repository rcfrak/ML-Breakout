/* This class calculates a score for each round based on the number of bricks 
 * broken thus far. The scorer communicates with ScoreStorage to write scores
 * at the end of a round (win or loss), and get a cumulative score
 */

using UnityEngine;

public class Scorer : MonoBehaviour
{
    private Observer observer;

    public const int scoreMultiplier = 10;
    private int score = 0;

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
        Debug.Log(getTotalScore());
    }

    public void writeWin()
    {
        ScoreStorage.Instance.addScore(score);
        ScoreStorage.Instance.addWin();
    }

    public void writeLoss()
    {
        ScoreStorage.Instance.addScore(score);
        ScoreStorage.Instance.gameLost();
    }

    public int getTotalScore()
    {
        return score + ScoreStorage.Instance.getSavedScore();
    }

}
