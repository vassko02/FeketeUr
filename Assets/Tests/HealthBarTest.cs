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

        Assert.AreEqual(maxHealth, healthBar.slider.maxValue, "Max érték nem megfelelõ.");
        Assert.AreEqual(maxHealth, healthBar.slider.value, "Aktuális érték nem megfelelõ, ha buff nincs.");
    }

    [Test]
    public void SetMaxHealth_WithBuff_SetsOnlyMaxValue()
    {
        int maxHealth = 100;

        healthBar.SetMaxHealth(maxHealth, true);

        Assert.AreEqual(maxHealth, healthBar.slider.maxValue, "Max érték nem megfelelõ.");
        Assert.AreNotEqual(maxHealth, healthBar.slider.value, "Aktuális érték nem megfelelõ buff esetén.");
    }

    [Test]
    public void SetHealth_UpdatesSliderValue()
    {
        int health = 75;

        healthBar.setHealth(health);

        Assert.AreEqual(health, healthBar.slider.value, "Életerõ érték nem megfelelõ.");
    }
}