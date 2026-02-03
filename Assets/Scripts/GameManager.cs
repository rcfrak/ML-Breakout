/*
    This is a static container to hold data between scene reloads, 
    prepared for use with the game play loop
    https://discussions.unity.com/t/i-need-to-save-the-score-when-the-scene-resets/855136/6
 */

using UnityEngine;

public static class GameManager
{
    private static int savedScore = 0;
    private static int roundsWon = 0;

    public static void addScore(int score)
    {
        savedScore += score;
    }

    public static void addWin()
    {
        roundsWon++;
    }

    public static void gameLost()
    {
        savedScore = 0;
        roundsWon = 0;
    }

    public static int getSavedScore()
    {
        return savedScore;
    }
}


