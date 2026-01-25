using UnityEngine;

public class Possession
{
    private GameObject targetEntity;
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

        Ray ray = DrawRayFromCrosshair();

        if (Physics.Raycast(ray, out RaycastHit hit, RaycastHitDistance))
        {
            HandlePossession(hit);
        }
        else
        {
            HandleDepossession();
        }
    }

    private void HandlePossession(RaycastHit hit)
    {
        IPossessable possessableEntity = hit.transform.GetComponent<IPossessable>();
        if (possessableEntity == null || possessableEntity == currentlyPossessed)
        {
            Debug.LogWarning($"Cannot Possess {currentlyPossessed}");
            return;
        }


        targetEntity = possessableEntity.GetPossessedEntity().gameObject;

        if (currentlyPossessed.GetPossessedEntity() is PlayerController)
        {
            if (possessableEntity is Enemy && !IsBehindEnemy(targetEntity)) return;
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