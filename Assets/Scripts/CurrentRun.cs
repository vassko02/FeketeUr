using System.IO;

[System.Serializable]
public class CurrentRun
{
    public string PlayerName;
    public int Score;
    public float ElapsedTime;
    public int MaxHealth;
    public int CurrentHealth;
    public int ScoreIncrement;

    public CurrentRun(string playerName, int Score,int MaxHealth,int currentHealth, int scoreIncrement,float ElapsedTime)
    {
        this.PlayerName = playerName;
        this.Score = Score;
        this.MaxHealth = MaxHealth;
        this.CurrentHealth = currentHealth;
        this.ScoreIncrement = scoreIncrement;
        this.ElapsedTime = ElapsedTime;
    }
    public CurrentRun()
    {
            
    }
}