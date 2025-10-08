using UnityEngine;

public class MainEntry : MonoBehaviour
{
    void Awake()
    {
        GameFlow.instance.Initialize();
    }

    void Start()
    {
        GameFlow.instance.PostInitialize();
    }

    void Update()
    {
        GameFlow.instance.Refresh(Time.deltaTime);
    }

    void FixedUpdate()
    {
        GameFlow.instance.PhysicsRefresh(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        GameFlow.instance.LateRefresh(Time.deltaTime);
    }

    private void OnDestroy()
    {
        GameFlow.instance.OnDemolish();
    }
}
