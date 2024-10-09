using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetMaxHealth(int health,bool buff)
    {
        slider.maxValue = health;
        if (!buff)
        {
            slider.value = health;

        }
    }
   public void setHealth(int health)
    {
        slider.value = health;
    }
}
