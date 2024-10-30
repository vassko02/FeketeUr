using System;
[System.Serializable]

public class HighScore
{
    public string PlayerName { get; private set; }
    public int Score { get; private set; }

    public HighScore(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}