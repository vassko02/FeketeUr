using System;

[System.Serializable]

public class HighScore
{
    public string PlayerName;
    public int Score;

    public HighScore(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}