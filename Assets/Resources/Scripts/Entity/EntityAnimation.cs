using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Animator entityAnimator;

    private Vector2 moveDir;

    public void SetSpeed(float speed)
    {
        float x = Mathf.Clamp01(speed);
        entityAnimator.SetFloat("Blend", x, .3f, Time.deltaTime);

        //Debug.Log($"animalAnimation: {this}, animator: {this.GetAnimator()}, speed: {speed}");
    }

    public void SetBlendDirection(Vector2 _moveDir)
    {
        moveDir = _moveDir;
        float x = Mathf.Clamp01(moveDir.magnitude);
        entityAnimator.SetFloat("Blend", x, .3f, Time.fixedDeltaTime);
    }

    public void SetBoolTransition(bool _isWalking)
    {
        entityAnimator.SetBool("Blend", _isWalking);
    }

    public Animator GetAnimator() => entityAnimator;
}
