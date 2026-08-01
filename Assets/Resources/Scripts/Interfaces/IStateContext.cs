public interface IStateContext
{
    public UnityEngine.AI.NavMeshAgent GetNavMeshAgent();
    public UnityEngine.Transform GetTransform();

    public EntityAnimation GetAnimationEntity();

    public abstract bool IsSafe();
    public abstract bool CanSeePossessedPlayer();
    public abstract bool CanSeePossessedAnimal();

    public abstract void ResetChanges();
    public abstract void ApplySettings(StateSettings _settings);
}