using UnityEngine;

public class AnimalNpc : Npc
{
    public enum animalType { Cat, Dog, Chicken, Tiger, Penguin, Horse, Deer }
    public animalType animal;

    IPuzzleObject puzzleObject;

    [SerializeField] private Gem gem;


    private void OnTriggerEnter(Collider other)
    {
        puzzleObject = other.transform.GetComponent<IPuzzleObject>();

        if (puzzleObject != null)
        {
            puzzleObject.Collected();
        }
    }
}
