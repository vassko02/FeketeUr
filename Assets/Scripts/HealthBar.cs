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
        fill.color=gradient.Evaluate(1f);
    }
   public void setHealth(int health)
    {
        slider.value = health;
        fill.color=gradient.Evaluate(slider.normalizedValue);
    }
}
