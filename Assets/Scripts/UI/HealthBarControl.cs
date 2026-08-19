using UnityEngine;
using UnityEngine.UI;

/* HealthBarControl: Controls UI slider for player health and updates fill color.
 - Awake(): cache fill image from slider.
 - SetSliderValue(float,float): update slider value and color using gradient.
*/
public class HealthBarControl : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    public Gradient gradient;
    private Image fillImage;

    void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }

    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        fillImage.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
