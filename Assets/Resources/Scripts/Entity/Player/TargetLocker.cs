using UnityEngine;

[System.Serializable]
public class TargetLocker
{
    [Header("Target Settings")] // Recommended Default Values
    [SerializeField] private float maxLockDistance = 25f;
    [SerializeField] private float maxScreenRadius = 0.05f;
    [SerializeField] private float lockFOVAngle = 10f;
    [SerializeField] private float closeRange = 1.25f;

    [SerializeField] private GameObject lockIndicatorPrefab;
    [SerializeField] private LayerMask possessableLayer;

    private GameObject currentActiveIndicator;
    private Transform currentLockedTarget;
    private Vector3 indicatorLocation;

    public void Initialize()
    {
        currentActiveIndicator = GameObject.Instantiate(lockIndicatorPrefab);
        ForceUnlock();
    }

    public void Refresh()
    {
        Transform best = FindBestTargetInView();

        if (best != currentActiveIndicator.transform)
        {
            SetLockedTarget(best);
        }
    }

    private void SetLockedTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            currentLockedTarget = newTarget;
            currentActiveIndicator.transform.SetParent(currentLockedTarget, false);
            currentActiveIndicator.transform.position = indicatorLocation;
            ToggleVisibility(true);
        }
        else
        {
            ToggleVisibility(false);
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

    private void ToggleVisibility(bool isVisible)
    {
        currentActiveIndicator.SetActive(isVisible);
    }

    public Transform GetCurrentLockedTarget()
    {
        if (currentActiveIndicator.activeInHierarchy)
            return currentLockedTarget;
        else
            return null;
    }

    public void ForceUnlock() => ToggleVisibility(false);

}