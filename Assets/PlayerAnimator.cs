using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private Animator playerAnimator;

    private const string IS_WALKING = "IsWalking";
    private const string IS_SPRINTING = "IsSprinting";

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!player.IsWalking())
        {
            playerAnimator.SetFloat("Blend", 0);
        }
        else
        {
            playerAnimator.SetFloat("Blend", player.transform.position.magnitude);
        }
    }
}
