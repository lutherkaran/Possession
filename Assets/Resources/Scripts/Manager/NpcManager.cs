using System.Collections.Generic;
using UnityEngine;

public class NpcManager : IManagable
{
    private static NpcManager Instance;
    public static NpcManager instance { get { return Instance == null ? Instance = new NpcManager() : Instance; } }

    private List<Npc> npcList = new List<Npc>();

    public void Initialize()
    {
        FindNpc();

        foreach (Npc npc in npcList)
        {
            npc.Initialize();
        }
    }

    void FindNpc()
    {
        Npc[] npcs = GameObject.FindObjectsByType<Npc>(FindObjectsSortMode.None); // Getting All NPCs
        
        for (int i = 0; i < npcs.Length; i++)
        {
            npcList.Add(npcs[i]);
        }   
    }

    public void LateRefresh(float deltaTime)
    {
        foreach (Npc npc in npcList)
        {
            npc.LateRefresh(deltaTime);
        }
    }

    public void OnDemolish()
    {
        foreach (Npc npc in npcList)
        {
            npc.OnDemolish();
        }

        Instance = null;
    }

    public void PhysicsRefresh(float fixedDeltaTime)
    {
        foreach (Npc npc in npcList)
        {
            npc.PhysicsRefresh(fixedDeltaTime);
        }
    }

    public void PostInitialize()
    {
        foreach (Npc npc in npcList)
        {
            npc.PostInitialize();
        }
    }

    public void Refresh(float deltaTime)
    {
        foreach (Npc npc in npcList)
        {
            npc.Refresh(deltaTime);
        }
    }
}
