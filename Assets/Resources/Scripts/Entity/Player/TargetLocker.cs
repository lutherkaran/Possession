using UnityEngine;

public class TargetLocker : MonoBehaviour
{
    [SerializeField] private float maxLockDistance = 40f;
    [SerializeField] private float maxLockAngle = 30f;

    [SerializeField] private GameObject lockIndicatorPrefab;

    [SerializeField] private LayerMask possessableLayer;

    private Transform currentLockedTarget;

    private GameObject activeIndicator;

    private void Update()
    {
        if (currentLockedTarget == null || !IsTargetStillValid(currentLockedTarget))
        {
            Transform best = FindBestTargetInView();
            SetLockedTarget(best);
        }
    }

    private bool IsTargetStillValid(Transform currentLockedTarget)
    {
        if (currentLockedTarget == null) return false;
        if (!currentLockedTarget.TryGetComponent<IPossessable>(out _)) return false;

        Vector3 dir = (currentLockedTarget.position - PlayerManager.instance.GetPlayer().transform.position).normalized;
        float dist = Vector3.Distance(CameraManager.instance.myCamera.transform.position, currentLockedTarget.position);
        float angle = Vector3.Angle(CameraManager.instance.myCamera.transform.forward, dir);

        return angle <= maxLockAngle && dist <= maxLockDistance;
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
        }
    }

    private Transform FindBestTargetInView()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, maxLockDistance, possessableLayer);
        float bestAngle = float.MaxValue;
        Transform best = null;

        Vector3 camPos = CameraManager.instance.myCamera.transform.position;
        Vector3 camForward = CameraManager.instance.myCamera.transform.forward;

        foreach (Collider col in hits)
        {
            if (!col.TryGetComponent<IPossessable>(out _)) continue;

            Vector3 dirToTarget = (col.transform.position - camPos).normalized;
            float angle = Vector3.Angle(camForward, dirToTarget);
            float dist = Vector3.Distance(camPos, col.transform.position);

            if (angle <= maxLockAngle && dist <= maxLockDistance && angle < bestAngle)
            {
                bestAngle = angle;
                best = col.transform;
            }
        }
        return best;
    }

    public Transform GetCurrentLockedTarget() => currentLockedTarget;

    public void ForceUnlock() => SetLockedTarget(null);
}