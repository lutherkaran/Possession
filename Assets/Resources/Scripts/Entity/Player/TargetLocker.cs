using UnityEngine;

public class TargetLocker : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 25f;
    [SerializeField] private float maxScreenRadius = 0.05f;

    [SerializeField] private GameObject lockIndicatorPrefab;

    [SerializeField] private LayerMask possessableLayer;
    [SerializeField] private float closeRange = 3f;

    private Transform currentLockedTarget;

    private GameObject activeIndicator;

    private Vector3 indicatorLocation;
    private float dist = 0;
    private float distanceFromCenter = 0;

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

        Collider[] hits = Physics.OverlapSphere(currentTransform.position, maxLockDistance, possessableLayer);
        float bestScore = -Mathf.Infinity;
        Transform best = null;

        foreach (Collider col in hits)
        {
            if (col.transform == currentTransform) continue;
            if (!col.TryGetComponent<IPossessable>(out IPossessable possessable)) continue;

            Vector3 screenPoint = CameraManager.instance.myCamera.WorldToViewportPoint(col.transform.position);

            distanceFromCenter = DistanceFromCenter(screenPoint);

            dist = Vector3.Distance(myCameraTransform.position, col.transform.position);

            bool isClose = dist <= closeRange;
            if ((distanceFromCenter <= maxScreenRadius || isClose) && dist <= maxLockDistance && HasLineOfSight(myCameraTransform, col))
            {
                float score = isClose ? 1f : -distanceFromCenter;

                if (score > bestScore)
                {
                    bestScore = score;
                    best = col.transform;
                    indicatorLocation = possessable.GetPossessedEntity().GetTargetLockerPoint().position;
                }
            }
        }
        return best;
    }

    private bool HasLineOfSight(Transform currentTransform, Collider col)
    {
        Vector3 dir = (col.bounds.center - currentTransform.position).normalized;

        if (Physics.Raycast(currentTransform.position, dir, out RaycastHit hit, maxLockDistance))
        {
            return hit.transform == col.transform;
        }
        return false;
    }

    private float DistanceFromCenter(Vector3 screenPoint)
    {
        return Vector2.Distance(new Vector2(0.5f, 0.5f), new Vector2(screenPoint.x, screenPoint.y));
    }

    public Transform GetCurrentLockedTarget() => currentLockedTarget;

    public void ForceUnlock() => SetLockedTarget(null);

}