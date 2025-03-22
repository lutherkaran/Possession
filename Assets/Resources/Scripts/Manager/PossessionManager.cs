using System;

public class PossessionManager
{
    private static PossessionManager instance;
    private Possession currentPossession;
    private IPossessable currentPossessable;

    public static PossessionManager Instance { get { return instance == null ? instance = new PossessionManager() : instance; } }
    public event EventHandler<IPossessable> OnPossessed;

    public Possession ToPossess(IPossessable possessible)
    {
        if (possessible != null && currentPossessable != possessible)
        {
            currentPossessable = possessible;
            CameraManager.instance?.AttachCameraToPossessedObject(currentPossessable.GetEntity().gameObject);
            currentPossession = new Possession(currentPossessable);
            OnPossessed?.Invoke(this, currentPossessable);
            return currentPossession;
        }

        return null;
    }

    public void ToDepossess(IPossessable possessable)
    {
        currentPossessable = possessable;
        if (currentPossessable != null)
        {
            currentPossessable = null;
            currentPossession = null;
        }

    }

    public Possession GetCurrentPossession() => currentPossession;

    public IPossessable GetCurrentPossessable() => currentPossessable;
}
