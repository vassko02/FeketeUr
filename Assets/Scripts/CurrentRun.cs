using System.IO;

public class CurrentRun
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public float ElapsedTime { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int ScoreIncrement { get; set; }

    public CurrentRun(string playerName, int Score,int MaxHealth,int currentHealth, int scoreIncrement,float ElapsedTime)
    {
        this.PlayerName = playerName;
        this.Score = Score;
        this.MaxHealth = MaxHealth;
        this.CurrentHealth = currentHealth;
        this.ScoreIncrement = scoreIncrement;
        this.ElapsedTime = ElapsedTime;
    }
}