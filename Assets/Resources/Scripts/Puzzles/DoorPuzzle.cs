using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : BasePuzzle
{
    public static Dictionary<IPuzzleObject, AnimalNpc.animalType> puzzleDictionary = new Dictionary<IPuzzleObject, AnimalNpc.animalType>();

    private int counter = 0;

    private void Start()
    {
        Gem.onPuzzlePieceCollected += DoorPuzzlePieceCollected;
    }

    private void DoorPuzzlePieceCollected(object sender, EventArgs e)
    {
        Debug.Log("Counter: " + ++counter);

        if (counter == 5)
        {
            Unlocked();
        }
    }

    private void Awake()
    {
        Locked();
    }

    protected override void Locked()
    {

    }

    protected override void Unlocked()
    {
        GetComponent<Animator>().SetBool("IsOpen", true);
    }

    protected override void Interacted()
    {
        if (puzzleDictionary.Count == 5)
        {
            Unlocked();
        }
    }
}
