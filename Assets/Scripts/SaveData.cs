using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public int score;
    public int maxHealth;
    public int currentHealth;
    public Vector3 playerPosition;
    public int currentLevel;
    public float progress;
}