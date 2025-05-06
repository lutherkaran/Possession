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
    [SerializeField] private float currentHealth;
    [SerializeField] private const float MAX_HEALTH = 100f;

    [Header("Damage Overlay")]
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float duration;
    [SerializeField] private float fadeSpeed; // How quickly the image will fade.
    [SerializeField] private float durationTimer;

    [SerializeField] private GameObject damagableObject;
    private IDamageable hasDamagable;


    private void Awake()
    {
        currentHealth = MAX_HEALTH;
    }

    private void Start()
    {
        hasDamagable = damagableObject.GetComponent<IDamageable>();
        if (hasDamagable == null) return;

        hasDamagable.OnDamaged += HasDamagable_OnDamaged;
        UpdateHealth(currentHealth);
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
    }

    private void HasDamagable_OnDamaged(object sender, IDamageable.OnDamagedEventArgs e)
    {
        HealthChange(e.health);
    }

    IEnumerator UpdateHealth(float health)
    {
        health = Mathf.Clamp(health, 0, MAX_HEALTH);

        while (lerpTimer < chipSpeed)
        {
            float fillF = frontHealthBar.fillAmount;
            float fillB = backHealthBar.fillAmount;
            float hFraction = health / MAX_HEALTH;

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
            yield return null;
        }
    }

    public void HealthChange(float healthChangeValue)
    {
        currentHealth += healthChangeValue;
        lerpTimer = 0;
        StartCoroutine(UpdateHealth(currentHealth));

        durationTimer = 0;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);
    }

    public float GetCurrentHealth() => currentHealth;

    private void DamageOverlay()
    {
        if (damageOverlay.color.a > 0)
        {
            if (currentHealth < 30) return;
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
