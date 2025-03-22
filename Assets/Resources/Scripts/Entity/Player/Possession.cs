using log4net.Util;
using UnityEngine;

public class Possession
{
    private GameObject targetEntity;
    private IPossessable currentPossession;
    private bool canPossess = true;
    public float RaycastHitDistance = 40.0f;

    public Possession(IPossessable possessed)
    {
        currentPossession = possessed;
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

        if (currentPossession.GetEntity() is PlayerController)
        {
            if (possessableEntity is Enemy && !IsBehindEnemy(targetEntity)) return;
        }
        // Perform possession
        possessableEntity.Possessing(targetEntity);
        currentPossession.GetEntity().StartCoroutine(CameraManager.instance.MovetoPosition(targetEntity));
        canPossess = false;
    }

    private bool IsBehindEnemy(GameObject enemy)
    {
        float dotProduct = Vector3.Dot(enemy.transform.forward.normalized, (currentPossession.GetEntity().transform.position - enemy.transform.position).normalized);

        return dotProduct < 0;
    }

    private void HandleDepossession()
    {
        if (targetEntity == null) return;

        canPossess = true;
        currentPossession.Depossessing(targetEntity);
    }

    public Ray DrawRayFromCamera()
    {
        Ray ray = CameraManager.instance.cam.ScreenPointToRay(Input.mousePosition);
        return ray;
    }
    
    public IPossessable GetCurrentPossession() => currentPossession;

    //public Ray DrawRayfromPlayerEye()
    //{
    //    Ray ray = new Ray(player.transform.position + (Vector3.up * 0.5f), player.transform.forward);
    //    Debug.DrawRay(ray.origin, ray.direction * 40, Color.red);
    //    return ray;
    //}
}