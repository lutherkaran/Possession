using UnityEngine;

public class Possession
{
    private GameObject targetEntity;
    private TargetLocker targetLocker;

    private IPossessable currentlyPossessed;
    private bool canPossess = true;

    private float RaycastHitDistance = 40.0f;

    public Possession(IPossessable possessed)
    {
        currentlyPossessed = possessed;
    }

    public void PossessEntities()
    {
        if (!canPossess) return;
        targetLocker = CameraManager.instance.GetMouseAim().GetTargetLocker();

        Ray ray = DrawRayFromCrosshair();
        
        Transform locked = targetLocker.GetCurrentLockedTarget();
        
        if(locked!=null && locked.TryGetComponent<IPossessable>(out var possessableCollider))
        {
            HandlePossession(possessableCollider);
        }
        else if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance) && hit.transform.TryGetComponent<IPossessable>(out var possessableRaycast))
        {
            HandlePossession(possessableRaycast);
        }
        else
        {
            HandleDepossession();
        }

        targetLocker.ForceUnlock();
    }

    private void HandlePossession(IPossessable possessable)
    {
        if (possessable == null || possessable == currentlyPossessed)
        {
            Debug.LogWarning($"Cannot Possess {currentlyPossessed}");
            return;
        }

        targetEntity = possessable.GetPossessedEntity().gameObject;

        if (currentlyPossessed.GetPossessedEntity() is PlayerController)
        {
            if (possessable is Enemy && !IsBehindEnemy(targetEntity)) return;
        }

        PossessionManager.instance.ToPossess(targetEntity);

        canPossess = false;
    }

    private bool IsBehindEnemy(GameObject enemy)
    {
        float dotProduct = Vector3.Dot(enemy.transform.forward.normalized, (currentlyPossessed.GetPossessedEntity().transform.position - enemy.transform.position).normalized);

        return dotProduct < 0;
    }

    private void HandleDepossession()
    {
        if (targetEntity == null) return;
        canPossess = true;
    }

    private Ray DrawRayFromCrosshair()
    {
        Ray ray = CameraManager.instance.myCamera.ScreenPointToRay(PlayerUI.Instance.GetCrosshairTransform().position);
        return ray;
    }

    public void RepossessPlayer(GameObject player)
    {
        PossessionManager.instance.ToPossess(player);
    }
}
