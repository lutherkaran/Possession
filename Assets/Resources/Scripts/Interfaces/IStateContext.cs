using UnityEngine.AI;

public interface IStateContext
{
    public NavMeshAgent GetNavMeshAgent();

    public abstract bool CanSeePlayer();
    public abstract void MakeChanges(StateSettings _settings);
    public abstract void ResetChanges();
}