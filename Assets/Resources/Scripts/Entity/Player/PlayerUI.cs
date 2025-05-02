using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private PlayerController player;

    private Transform crosshairTransform;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PlayerUI in the scene");
        }
        Instance = this;

        crosshairTransform = GetComponentInChildren<Transform>();
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public Transform GetCrosshairTransform()
    {
        return crosshairTransform;
    }
}
