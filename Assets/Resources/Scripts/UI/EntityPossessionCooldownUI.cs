using UnityEngine;
using UnityEngine.UI;

/*
 Summary: This class implements a cooldown logic i.e unique to all entities For example when a player possesses an entity then during this
 cooldown the player cannot possess another entity and this cooldown timer is different for each entity.
 */

public class EntityPossessionCooldownUI : MonoBehaviour
{
    public static EntityPossessionCooldownUI Instance { get; private set; }

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
        PossessionManager.instance.OnPossessed += SetTimer_OnPossessed;
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
            cooldownTimerImage.fillAmount = 1 - cooldownTimer / possessionCooldownTimerMax;

            if (cooldownTimer >= possessionCooldownTimerMax)
            {
                cooldownTimerImage.fillAmount = 1;
                cooldownTimer = 0;

                coolingDown = false;
            }
        }
    }

    public bool GetCoolingDown() => coolingDown;
}
