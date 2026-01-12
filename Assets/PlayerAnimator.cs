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
        if (player.IsWalking())
        {
            playerAnimator.SetBool(IS_WALKING, player.IsWalking());
        }
        else
            playerAnimator.SetBool(IS_SPRINTING, player.isSprinting());
    }
}
