using log4net.Util;
using UnityEngine;

public class Possession
{
    private PlayerController player;
    private GameObject targetEntity;
    private IPossessable currentPossession;
    private bool canPossess = true;
    public float RaycastHitDistance = 40.0f;

    public Possession(PlayerController playerController)
    {
        this.player = playerController;
    }

    public void PossessEntities()
    {
        if (!canPossess)
        {
            HandleDepossession();
            return;
        }

        Ray ray = DrawRayFromCamera();

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
        var possessableEntity = hit.transform.GetComponentInParent<IPossessable>();
        targetEntity = hit.transform.GetComponentInParent<Entity>()?.gameObject;

        if (possessableEntity == null) return;

        if (possessableEntity is Enemy && !IsBehindEnemy(targetEntity)) return;

        // Perform possession
        possessableEntity.Possess(targetEntity);
        player.StartCoroutine(CameraManager.instance.MovetoPosition(targetEntity));

        currentPossession = possessableEntity;
        player.playerPossessed = null;
        canPossess = false;
    }

    private bool IsBehindEnemy(GameObject enemy)
    {
        float dotProduct = Vector3.Dot(enemy.transform.forward.normalized, (player.transform.position - enemy.transform.position).normalized);

        return dotProduct < 0;
    }

    private void HandleDepossession()
    {
        if (targetEntity == null) return;

        canPossess = true;
        currentPossession.Depossess(targetEntity);
        currentPossession = player.playerPossessed = PossessionManager.Instance.ToPossess(player);
    }

    public Ray DrawRayfromPlayerEye()
    {
        Ray ray = new Ray(player.transform.position + (Vector3.up * 0.5f), player.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);
        return ray;
    }

    public Ray DrawRayFromCamera()
    {
        Ray ray = CameraManager.instance.cam.ScreenPointToRay(Input.mousePosition);
        return ray;
    }

}