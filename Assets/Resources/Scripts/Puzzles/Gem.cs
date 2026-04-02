using System;
using UnityEngine;

public class Gem : MonoBehaviour, IPuzzleObject
{
    [SerializeField] private GemsSO gemsSO;

    public static event EventHandler onPuzzlePieceCollected;

    private void Start()
    {
        DoorPuzzle.puzzleDictionary.Add(this, gemsSO.animal);
    }

    public void HasPuzzleObject()
    {
        Debug.Log(gemsSO.animal.ToString());
    }

    public GemsSO GetGemSO()
    {
        return gemsSO;
    }

    public void Collected()
    {
        Debug.Log("Collected");
        onPuzzlePieceCollected?.Invoke(this, EventArgs.Empty);
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
