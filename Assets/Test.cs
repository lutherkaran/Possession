using UnityEngine;

public class Test : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Blend", Time.deltaTime, .5f, Time.deltaTime);
    }
}
