public interface IStateContext
{
    public UnityEngine.AI.NavMeshAgent GetNavMeshAgent();
    public UnityEngine.Transform GetTransform();

    public abstract bool IsSafe();
    public abstract bool CanSeePlayer();

    public abstract void ResetChanges();
    public abstract void ApplySettings(StateSettings _settings);
}