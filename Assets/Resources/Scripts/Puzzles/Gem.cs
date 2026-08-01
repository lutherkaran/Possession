using System;
using UnityEngine;

public class Gem : MonoBehaviour, IPuzzleObject
{
    [SerializeField] private GemsSO gemsSO;

    public static event EventHandler onPuzzlePieceCollected;

    // Registration moved from Start() (add-only, never removed) to OnEnable/OnDisable so the
    // static dictionary doesn't accumulate stale/destroyed keys across scene reloads or repeated
    // play sessions -- Hide() below disables the object, which now cleanly unregisters it too.
    private void OnEnable()
    {
        if (gemsSO != null && !DoorPuzzle.puzzleDictionary.ContainsKey(this))
            DoorPuzzle.puzzleDictionary.Add(this, gemsSO.animal);
    }

    private void OnDisable()
    {
        DoorPuzzle.puzzleDictionary.Remove(this);
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