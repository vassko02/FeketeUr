using TMPro;
using UnityEngine;

public class LeaderBoardItem : MonoBehaviour
{
    public TMP_Text rank, name, score;
    public void Init(string rankValue, string nameValue, string scoreValue)
    {
        rank.text = rankValue;
        name.text = nameValue;
        score.text = scoreValue;
    }
}
