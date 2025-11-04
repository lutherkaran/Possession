using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzle : BasePuzzle
{
    public static Dictionary<AnimalNpc.animalType, IPuzzleObject> puzzleDictionary = new Dictionary<AnimalNpc.animalType, IPuzzleObject>();

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
            GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }

    private void Awake()
    {
        Locked();
    }

    protected override void Locked()
    {
        // if it's a correct entity that can interact to this object?
        // TRUE: 
        // if entity has that missing object to solve this puzzle?
        // TRUE:
        // Door Unlocked.
        // FALSE:
        // Object is incorrect
        // else Print a message or puzzle hint
        // else This is not the correct entity to intract to that puzzle   
    }

    protected override void Unlocked()
    {
        throw new System.NotImplementedException();
    }

    protected override void Interacted()
    {
        if (puzzleDictionary.Count == 5)
        {
            Unlocked();
        }
    }
}
