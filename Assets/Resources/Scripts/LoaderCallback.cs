using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    public InputManager inputManager { get; private set; } 
    public PossessionManager possessionManager { get; private set; }
    public EntityManager entityManager { get; private set; }
    public BulletManager bulletManager { get; private set; }
    public GameManager gameManager { get; private set; }
    public PlayerController playerController { get; private set; }
   
    bool isFirstFrame = true;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            Loader.LoaderCallback();
        }
    }
}
