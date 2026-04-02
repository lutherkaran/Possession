using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    protected abstract void Locked();
    protected abstract void Unlocked();
    protected abstract void Interacted();
}
