using NUnit.Framework;
using TMPro;
using UnityEngine;

public class LeaderBoardItemTest
{
    private LeaderBoardItem leaderBoardItem;

    [SetUp]
    public void Setup()
    {
        // L�trehozzuk a LeaderBoardItem objektumot
        var leaderBoardItemObj = new GameObject("LeaderBoardItem");
        leaderBoardItem = leaderBoardItemObj.AddComponent<LeaderBoardItem>();

        // L�trehozzuk �s be�ll�tjuk a TMP_Text mez�ket
        leaderBoardItem.rank = CreateTextMeshPro("Rank");
        leaderBoardItem.name = CreateTextMeshPro("Name");
        leaderBoardItem.score = CreateTextMeshPro("Score");
    }

    // Seg�df�ggv�ny egy TMP_Text objektum l�trehoz�s�hoz
    private TMP_Text CreateTextMeshPro(string name)
    {
        var textObj = new GameObject(name);
        return textObj.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void Init_SetsTextFieldsCorrectly()
    {
        // Teszt adatok
        string rankValue = "1";
        string nameValue = "PlayerOne";
        string scoreValue = "1000";

        // Init met�dus megh�v�sa
        leaderBoardItem.Init(rankValue, nameValue, scoreValue);

        // Ellen�rizz�k, hogy a mez�k �rt�kei helyesek-e
        Assert.AreEqual(rankValue, leaderBoardItem.rank.text);
        Assert.AreEqual(nameValue, leaderBoardItem.name.text);
        Assert.AreEqual(scoreValue, leaderBoardItem.score.text);
    }
}