using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleObjects")]
public class GemsSO : ScriptableObject
{
    public GameObject gemPrefab;
    public string gemName;
    public AnimalNpc.animalType animal;
}
