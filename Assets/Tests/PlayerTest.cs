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
        player.healthBar = new GameObject().AddComponent<HealthBar>();  // Felt�telezve, hogy van egy HealthBar komponens
        player.currentHealth = 100;
        player.maxHealt = 100;
        player.damageBuffUIImage = new GameObject();
        player.shieldBuffUIImage= new GameObject();
        player.BuffUI = new GameObject();
        healthBar = new GameObject("HealthBar").AddComponent<HealthBar>();
        
        // Inicializ�ljuk a Player komponens sz�ks�ges param�tereit
        player.maxHealt = 100;
        player.currentHealth = 100; // Kezdeti �let
        player.healthBar = healthBar;

    }

    [Test]
    public void TakeDamage_WithShieldBuff_DoesNotTakeDamage()
    {
        // Aktiv�ljuk a pajzs buffot
        player.ActivateShieldBuff();

        // Elmentj�k a j�t�kos aktu�lis eg�szs�g�t
        int initialHealth = player.currentHealth;

        // Sebz�st okozunk
        player.TakeDamage(50);

        // Ellen�rizz�k, hogy nem v�ltozott az eg�szs�g
        Assert.AreEqual(initialHealth, player.currentHealth);
    }
    [Test]
    public void Heal_IncreasesHealth()
    {
        // El�sz�r megh�vjuk a Heal met�dust 30 HP-val
        player.Heal(30);

        // Ellen�rizz�k, hogy az �let a v�rt m�don n�tt-e
        Assert.AreEqual(80, player.currentHealth, "A Heal met�dus nem n�velte megfelel�en az �letet.");
    }

    [Test]
    public void Heal_DoesNotIncreaseHealthAboveMaxHealth()
    {
        // Be�ll�tjuk a maxim�lis �leter�t
        player.maxHealt = 100;
        player.currentHealth = 90;

        // Megh�vjuk a Heal met�dust 20 HP-val
        player.Heal(20);

        // Ellen�rizz�k, hogy nem l�pte t�l a maxim�lis �leter�t
        Assert.AreEqual(100, player.currentHealth, "A Heal met�dus nem korl�tozta megfelel�en az �letet.");
    }

    [Test]
    public void Test_AddToScore_IncreasesScoreCorrectly()
    {
        // Kezdeti pontsz�m ellen�rz�se
        Assert.AreEqual(0, player.score, "A kezd�pontsz�m nem 0.");

        // Hozz�ad�s 10 ponttal
        player.AddToScore(10);

        // Ellen�rizz�k, hogy a pontsz�m 10-tel n�tt
        Assert.AreEqual(10, player.score, "A pontsz�m nem n�vekedett helyesen.");

        // Hozz�ad�s egy �jabb 5 ponttal
        player.AddToScore(5);

        // Ellen�rizz�k, hogy a pontsz�m most 15
        Assert.AreEqual(15, player.score, "A pontsz�m nem n�vekedett helyesen a m�sodik hozz�ad�s ut�n.");
    }

    [TearDown]
    public void TearDown()
    {
        // A teszt ut�n tiszt�tjuk a j�t�kobjektumokat
        Object.DestroyImmediate(player.gameObject);
        Object.DestroyImmediate(healthBar.gameObject);
    }
}
