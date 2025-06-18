using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PossessionCooldownUI : MonoBehaviour
{
    public static PossessionCooldownUI Instance { get; private set; }

    [SerializeField] private Image cooldownTimerImage;
    [SerializeField] private PlayerController player;

    private float possessionCooldownTimerMax;
    private float cooldownTimer;

    private bool coolingDown = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
        cooldownTimerImage.fillAmount = 0;
    }

    public void Start()
    {
        PossessionManager.Instance.OnPossessed += SetTimer_OnPossessed;
    }

    private void SetTimer_OnPossessed(object sender, IPossessable e)
    {
        possessionCooldownTimerMax = e.GetPossessedEntity().GetPossessionCooldownTimerMax();
        coolingDown = true;
    }

    private void Update()
    {
        CooldownTimer();
    }

    private void CooldownTimer()
    {
        if (coolingDown)
        {
            cooldownTimer += Time.deltaTime;
            cooldownTimerImage.fillAmount = cooldownTimer / possessionCooldownTimerMax;

            if (cooldownTimer >= possessionCooldownTimerMax)
            {
                cooldownTimerImage.fillAmount = 0;
                cooldownTimer = 0;

                coolingDown = false;
            }
        }
    }

    public bool GetCoolingDown() => coolingDown;
}
