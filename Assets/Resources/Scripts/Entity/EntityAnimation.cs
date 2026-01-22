using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Animator entityAnimator;

    public Animator GetAnimator() => entityAnimator;

    public void SetSpeed(float speed)
    {
        GetAnimator().SetFloat("Blend", speed, 1f, Time.deltaTime);

        //Debug.Log($"animalAnimation: {this}, animator: {this.GetAnimator()}, speed: {speed}");
    }
}
