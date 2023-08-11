using UnityEngine;

public class FollowPossessedEntity : MonoBehaviour
{
    Vector3 offset = new Vector3(0, 5, -5f);
    bool followed = false;

    void Update()
    {
        if (PossessionManager.currentlyPossessed != null)
        {
            var f = PossessionManager.go.transform;

            if (f && !followed)
            {
                FollowPossessed(f);
            }
        }
    }

    private void FollowPossessed(Transform f)
    {
        this.transform.position = f.transform.position + offset;
        //this.transform.SetParent(f);
    }
}
