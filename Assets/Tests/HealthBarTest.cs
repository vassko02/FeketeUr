using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class HealthBarTest
{
    private HealthBar healthBar;
    private Slider slider;

    [SetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject();
        healthBar = gameObject.AddComponent<HealthBar>();

        slider = new GameObject().AddComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = 100;

        healthBar.slider = slider;

        healthBar.fill = new GameObject().AddComponent<Image>();
    }

    [Test]
    public void SetMaxHealth_WithNoBuff_SetsMaxValueAndCurrentValue()
    {
        int maxHealth = 100;

        healthBar.SetMaxHealth(maxHealth, false);

        Assert.AreEqual(maxHealth, healthBar.slider.maxValue, "Max �rt�k nem megfelel�.");
        Assert.AreEqual(maxHealth, healthBar.slider.value, "Aktu�lis �rt�k nem megfelel�, ha buff nincs.");
    }

    [Test]
    public void SetMaxHealth_WithBuff_SetsOnlyMaxValue()
    {
        int maxHealth = 100;

        healthBar.SetMaxHealth(maxHealth, true);

        Assert.AreEqual(maxHealth, healthBar.slider.maxValue, "Max �rt�k nem megfelel�.");
        Assert.AreNotEqual(maxHealth, healthBar.slider.value, "Aktu�lis �rt�k nem megfelel� buff eset�n.");
    }

    [Test]
    public void SetHealth_UpdatesSliderValue()
    {
        int health = 75;

        healthBar.setHealth(health);

        Assert.AreEqual(health, healthBar.slider.value, "�leter� �rt�k nem megfelel�.");
    }
}