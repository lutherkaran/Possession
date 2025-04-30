using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private float lerpTimer;

    [Header("Health Bar")]
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float duration;
    public float fadeSpeed; // How quickly the image will fade.
    private float durationTimer;

    private void Start()
    {
        if (player == null) return;
        player.health = player.maxHealth;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);
    }

    private void Update()
    {
        if (player == null) return;
        player.health = Mathf.Clamp(player.health, 0, player.maxHealth);
        if (player.health > 0)
        {
            UpdateHealthUI();
        }
        //DamageOverlay();
    }

    private void DamageOverlay()
    {
        if (damageOverlay.color.a > 0)
        {
            if (player.health < 30) return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }
    }

    private void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = player.health / player.maxHealth;

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

    public void TakeDamage(float damage)
    {
        player.health -= damage;
        lerpTimer = 0;
        durationTimer = 0;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);
    }
    public void RestoreHealth(float healAmount)
    {
        player.health += healAmount;
        lerpTimer = 0f;
    }
}
