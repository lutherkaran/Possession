using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private float chipSpeed = 2f;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private float lerpTimer;

    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [Header("Damage Overlay")]
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float duration;
    [SerializeField] private float fadeSpeed; // How quickly the image will fade.
    [SerializeField] private float durationTimer;

    [SerializeField] private GameObject damagableObject;
    private IDamageable hasDamagable;

    private void Start()
    {
        hasDamagable = damagableObject.GetComponent<IDamageable>();

        if (hasDamagable == null) return;

        health = maxHealth = hasDamagable.GetMaxHealth();

        hasDamagable.OnDamaged += HasDamagable_OnDamaged;

        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
    }

    private void HasDamagable_OnDamaged(object sender, IDamageable.OnDamagedEventArgs e)
    {
        HealthChange(e.health);
    }

    private void Update()
    {
        UpdateHealth(health);
    }

    private void UpdateHealth(float health)
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

    }

    private void HealthChange(float healthChangeValue)
    {
        health += healthChangeValue;
        lerpTimer = 0;

        if (health > 100 || health < 0)
            health = Mathf.Clamp(health, 0, maxHealth);

        durationTimer = 0;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);

    }

    public float GetHealth() => health;

    private void DamageOverlay()
    {
        if (damageOverlay.color.a > 0)
        {
            if (health < 30) return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }
    }
}
