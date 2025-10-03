using System;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    public event EventHandler<IPossessable> OnPossessed;

    public static PossessionManager instance { get; private set; }

    private Possession currentPossession;
    private IPossessable currentPossessable;

    private PlayerController playerController;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance); instance = null;
        }

        instance = this;
    }

    private void Start()
    {
        playerController = PlayerController.instance.GetPlayer();
        ToPossess(playerController.gameObject);
    }

    public Possession ToPossess(GameObject possessable)
    {
        if (possessable)
        {
            if (currentPossessable != null)
                ToDepossess(currentPossessable.GetPossessedEntity().gameObject);

            currentPossessable = possessable.GetComponent<IPossessable>();
            currentPossessable.Possessing(possessable);

            currentPossession = new Possession(currentPossessable);
            OnPossessed?.Invoke(this, currentPossessable);
            return currentPossession;
        }

        return null;
    }

    public void ToDepossess(GameObject possessable)
    {
        currentPossessable = possessable.GetComponent<IPossessable>();
        currentPossessable.Depossessing(possessable);

        if (currentPossessable != null)
        {
            currentPossessable = null;
            currentPossession = null;
        }
    }

    public Possession GetCurrentPossession() => currentPossession;

    public IPossessable GetCurrentPossessable() => currentPossessable;
}
