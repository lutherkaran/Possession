using UnityEngine;
using UnityEngine.UI;

public class EntityDefaultPossessionTimer : MonoBehaviour
{
    [SerializeField] private float defaultPossessionTimerMax;
    [SerializeField] private Image defaultPossessionTimerBar;
    [SerializeField] private PlayerController player;
    [SerializeField] private Pickups pickups;

    private float defaultPossessionTimer;

    private void Awake()
    {
        defaultPossessionTimerBar.fillAmount = 1f;
    }

    private void Start()
    {
        pickups.OnInteract += Pickups_OnInteract;
        Hide();
    }

    private void Pickups_OnInteract(object sender, Pickups.OnInteractEventArgs e)
    {
        Show();
        defaultPossessionTimerMax += e.possessionTimer;
        defaultPossessionTimer = defaultPossessionTimerMax;
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (defaultPossessionTimerMax > 0)
        {
            defaultPossessionTimer -= Time.deltaTime;
            defaultPossessionTimerBar.fillAmount = Mathf.Clamp01(defaultPossessionTimer / defaultPossessionTimerMax);

            if (defaultPossessionTimer <= 0)
            {
                defaultPossessionTimerMax = 0;
                defaultPossessionTimerBar.fillAmount = 0;
                defaultPossessionTimer = 1f;

                //PossessionManager.Instance.GetCurrentPossession()?.RepossessPlayer(player.gameObject);
                Hide();
            }
        }
    }
}
