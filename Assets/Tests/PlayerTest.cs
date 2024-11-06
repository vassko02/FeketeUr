using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[TestFixture]
public class PlayerTests
{
    private GameObject playerObject;
    private Player player;
    private HealthBar healthBar;
    [SetUp]
    public void SetUp()
    {
        playerObject = new GameObject();
        player = playerObject.AddComponent<Player>();
        player.healthBar = new GameObject().AddComponent<HealthBar>();  // Feltételezve, hogy van egy HealthBar komponens
        player.currentHealth = 100;
        player.maxHealt = 100;
        player.damageBuffUIImage = new GameObject();
        player.shieldBuffUIImage= new GameObject();
        player.BuffUI = new GameObject();
        healthBar = new GameObject("HealthBar").AddComponent<HealthBar>();
        
        // Inicializáljuk a Player komponens szükséges paramétereit
        player.maxHealt = 100;
        player.currentHealth = 100; // Kezdeti élet
        player.healthBar = healthBar;

    }

    [Test]
    public void TakeDamage_WithShieldBuff_DoesNotTakeDamage()
    {
        // Aktiváljuk a pajzs buffot
        player.ActivateShieldBuff();

        // Elmentjük a játékos aktuális egészségét
        int initialHealth = player.currentHealth;

        // Sebzést okozunk
        player.TakeDamage(50);

        // Ellenõrizzük, hogy nem változott az egészség
        Assert.AreEqual(initialHealth, player.currentHealth);
    }
    [Test]
    public void Heal_IncreasesHealth()
    {
        // Elõször meghívjuk a Heal metódust 30 HP-val
        player.Heal(30);

        // Ellenõrizzük, hogy az élet a várt módon nõtt-e
        Assert.AreEqual(80, player.currentHealth, "A Heal metódus nem növelte megfelelõen az életet.");
    }

    [Test]
    public void Heal_DoesNotIncreaseHealthAboveMaxHealth()
    {
        // Beállítjuk a maximális életerõt
        player.maxHealt = 100;
        player.currentHealth = 90;

        // Meghívjuk a Heal metódust 20 HP-val
        player.Heal(20);

        // Ellenõrizzük, hogy nem lépte túl a maximális életerõt
        Assert.AreEqual(100, player.currentHealth, "A Heal metódus nem korlátozta megfelelõen az életet.");
    }

    [Test]
    public void Test_AddToScore_IncreasesScoreCorrectly()
    {
        // Kezdeti pontszám ellenõrzése
        Assert.AreEqual(0, player.score, "A kezdõpontszám nem 0.");

        // Hozzáadás 10 ponttal
        player.AddToScore(10);

        // Ellenõrizzük, hogy a pontszám 10-tel nõtt
        Assert.AreEqual(10, player.score, "A pontszám nem növekedett helyesen.");

        // Hozzáadás egy újabb 5 ponttal
        player.AddToScore(5);

        // Ellenõrizzük, hogy a pontszám most 15
        Assert.AreEqual(15, player.score, "A pontszám nem növekedett helyesen a második hozzáadás után.");
    }

    [TearDown]
    public void TearDown()
    {
        // A teszt után tisztítjuk a játékobjektumokat
        Object.DestroyImmediate(player.gameObject);
        Object.DestroyImmediate(healthBar.gameObject);
    }
}
