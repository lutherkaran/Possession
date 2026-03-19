using UnityEngine;

public class TargetLocker : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 25f;
    [SerializeField] private float maxScreenRadius = 0.05f;
    [SerializeField] private float lockFOVAngle = 45f;

    [SerializeField] private GameObject lockIndicatorPrefab;

    [SerializeField] private LayerMask possessableLayer;
    [SerializeField] private float closeRange = 3f;

    private Transform currentLockedTarget;

    private GameObject activeIndicator;

    private Vector3 indicatorLocation;

    private void Update()
    {
        Transform best = FindBestTargetInView();

        if (best != currentLockedTarget)
        {
            SetLockedTarget(best);
        }
    }

    private void SetLockedTarget(Transform newTarget)
    {
        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
            activeIndicator = null;
        }

        currentLockedTarget = newTarget;

        if (newTarget != null && lockIndicatorPrefab != null)
        {
            activeIndicator = Instantiate(lockIndicatorPrefab, newTarget);
            activeIndicator.transform.position = indicatorLocation;
        }
    }

    private Transform FindBestTargetInView()
    {
        Transform currentTransform = PossessionManager.instance.GetCurrentPossessable().GetPossessedEntity().transform;
        Transform myCameraTransform = CameraManager.instance.myCamera.transform;
        Transform best = null;

        float bestScore = -Mathf.Infinity;

        Collider[] hits = Physics.OverlapSphere(currentTransform.position, maxLockDistance, possessableLayer);
        foreach (Collider col in hits)
        {
            if (col.transform == currentTransform) continue;
            if (!col.TryGetComponent<IPossessable>(out IPossessable possessable)) continue;

            Vector3 dirToTarget = (col.transform.position - myCameraTransform.position).normalized;

            float angleToTarget = Vector3.Angle(myCameraTransform.forward, dirToTarget);
            if (angleToTarget > lockFOVAngle) continue;

            float dist = Vector3.Distance(myCameraTransform.position, col.transform.position);
            bool isClose = dist <= closeRange;

            if (dist <= maxLockDistance && HasLineOfSight(myCameraTransform, col))
            {
                float score = isClose ? 1f : (lockFOVAngle - angleToTarget);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = col.transform;
                    indicatorLocation = possessable.GetPossessedEntity().GetTargetLockTransform().position;
                }
            }
        }
        return best;
    }

    private bool HasLineOfSight(Transform myCameraTransform, Collider col)
    {
        Vector3 dir = (col.bounds.center - myCameraTransform.position).normalized;

        if (Physics.Raycast(myCameraTransform.position, dir, out RaycastHit hit, maxLockDistance))
        {
            return hit.transform == col.transform;
        }
        return false;
    }

    public Transform GetCurrentLockedTarget() => currentLockedTarget;

    public void ForceUnlock() => SetLockedTarget(null);

}