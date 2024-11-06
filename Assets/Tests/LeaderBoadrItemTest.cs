using NUnit.Framework;
using TMPro;
using UnityEngine;

public class LeaderBoardItemTest
{
    private LeaderBoardItem leaderBoardItem;

    [SetUp]
    public void Setup()
    {
        // Létrehozzuk a LeaderBoardItem objektumot
        var leaderBoardItemObj = new GameObject("LeaderBoardItem");
        leaderBoardItem = leaderBoardItemObj.AddComponent<LeaderBoardItem>();

        // Létrehozzuk és beállítjuk a TMP_Text mezõket
        leaderBoardItem.rank = CreateTextMeshPro("Rank");
        leaderBoardItem.name = CreateTextMeshPro("Name");
        leaderBoardItem.score = CreateTextMeshPro("Score");
    }

    // Segédfüggvény egy TMP_Text objektum létrehozásához
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

        // Init metódus meghívása
        leaderBoardItem.Init(rankValue, nameValue, scoreValue);

        // Ellenõrizzük, hogy a mezõk értékei helyesek-e
        Assert.AreEqual(rankValue, leaderBoardItem.rank.text);
        Assert.AreEqual(nameValue, leaderBoardItem.name.text);
        Assert.AreEqual(scoreValue, leaderBoardItem.score.text);
    }
}