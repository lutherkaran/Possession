using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * For how long a player can possess an entity and keep the possession. if the player posseses an entity having 10s then possession time is of 10s and in this 10s player can possess other
 * entities before getting depossessed automatically.
 */
public class PossessionTimerUI : MonoBehaviour
{
    [SerializeField] private Image possessionTimerImage;
    [SerializeField] private PlayerController player;

    private float entityPossessionTimerMax;
    private float defaultPossessionTimerMax;
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
            defaultPossessionTimerMax -= Time.deltaTime;
            if (defaultPossessionTimerMax <= 0)
            {
                PossessionManager.Instance.GetCurrentPossession()?.RepossessPlayer(player.gameObject);
            }
            else
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
        defaultPossessionTimerMax = 25f;
        possessionTimerImage.fillAmount = 1f;
        possessionTimer = 1f;
    }
}
