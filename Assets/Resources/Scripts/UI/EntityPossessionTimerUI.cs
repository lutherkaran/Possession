using UnityEngine;
using UnityEngine.UI;

/*
 * For how long a player can possess an entity and keep the possession. if the player posseses an entity having 10s 
 * then possession time is of 10s so the player will deposess the enitity automatically and reposesss itself.
 */

public class EntityPossessionTimerUI : MonoBehaviour
{
    [SerializeField] private Image possessionTimerImage;
    [SerializeField] private PlayerController player;

    private float entityPossessionTimerMax;
    private float possessionTimer;

    private bool possession = false;
    private bool isTimerOn = false;

    private void Awake()
    {
        ResetFillAmount();
    }

    public void Start()
    {
        PossessionManager.Instance.OnPossessed += SetTimer_OnPossessed;
    }

    private void SetTimer_OnPossessed(object sender, IPossessable e)
    {
        if (PlayerController.Instance.isPossessed) { ResetFillAmount(); return; }

        possession = true;

        if (!isTimerOn)
        {
            entityPossessionTimerMax = e.GetPossessedEntity().GetEntityPossessionTimerMax();
            possessionTimer = entityPossessionTimerMax;
            isTimerOn = true;
        }
    }

    // When player possesses an Entities Longer than the TimerMax then player should Depossess the entity after that time;
    private void Update()
    {
        PossessionTimer();
    }

    private void PossessionTimer()
    {
        if (possession)
        {
            {
                if (isTimerOn)
                {
                    possessionTimer -= Time.deltaTime;
                    possessionTimerImage.fillAmount = possessionTimer / entityPossessionTimerMax;

                    if (possessionTimer <= 0)
                    {
                        PossessionManager.Instance.GetCurrentPossession()?.RepossessPlayer(player.gameObject);
                        possessionTimerImage.fillAmount = 100 / entityPossessionTimerMax;
                        possessionTimer = entityPossessionTimerMax;

                        possession = false;
                        isTimerOn = false;
                    }
                }
            }
        }
    }

    private void ResetFillAmount()
    {
        isTimerOn = false;
        possession = false;
        possessionTimerImage.fillAmount = 1f;
        possessionTimer = 1f;
    }
}
