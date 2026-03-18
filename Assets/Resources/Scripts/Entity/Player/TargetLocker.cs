using UnityEngine;

public class TargetLocker : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 25f;
    [SerializeField] private float maxScreenRadius = 0.05f;

    [SerializeField] private GameObject lockIndicatorPrefab;

    [SerializeField] private LayerMask possessableLayer;

    private Transform currentLockedTarget;

    private GameObject activeIndicator;

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
            activeIndicator = Instantiate(lockIndicatorPrefab, newTarget.position + new Vector3(0, 0.55f, 0), Quaternion.identity, newTarget);
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

            Vector3 toTarget = (col.transform.position - myCameraTransform.position).normalized;
            Vector3 screenPoint = CameraManager.instance.myCamera.WorldToViewportPoint(col.transform.position);

            float distanceFromCenter = DistanceFromCenter(screenPoint);

            float dist = Vector3.Distance(myCameraTransform.position, col.transform.position);

            if (distanceFromCenter <= maxScreenRadius && dist <= maxLockDistance)
            {
                float score = -distanceFromCenter; // closer to center = better

                if (score > bestScore)
                {
                    bestScore = score;
                    best = col.transform;
                }
            }

        }
        return best;
    }

    private float DistanceFromCenter(Vector3 screenPoint)
    {
        return Vector2.Distance(new Vector2(0.5f, 0.5f),new Vector2(screenPoint.x, screenPoint.y));
    }

    public Transform GetCurrentLockedTarget() => currentLockedTarget;

    public void ForceUnlock() => SetLockedTarget(null);
}