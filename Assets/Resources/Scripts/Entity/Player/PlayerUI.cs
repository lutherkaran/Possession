using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI promptText;
    [SerializeField] private PlayerController player;

    void Start()
    {

    }

    private void Update()
    {

    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
