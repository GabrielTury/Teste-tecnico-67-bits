using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public static NpcSpawner instance;

    [SerializeField]
    private float spawnRadius;

    int npcCount = 0;
    [SerializeField]
    int npcLimit = 0;

    [SerializeField]
    private GameObject Npc;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < npcLimit; i++) 
        {         
            SpawnNpc();
        }
    }

    /// <summary>
    /// Remove o NPC do total para spawna-lo de novo
    /// </summary>
    public void RemoveNpc()
    {
        npcCount--;
        if (npcCount < npcLimit)
            SpawnNpc();
    }
    /// <summary>
    /// Spawna o NPC
    /// </summary>
    private void SpawnNpc()
    {
        
        if(npcCount < npcLimit)
        {
            Vector3 randomPos = new Vector3(Random.Range(-spawnRadius,spawnRadius),0,Random.Range(-spawnRadius,spawnRadius));
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            Instantiate(Npc,randomPos,randomRot);
            npcCount++;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius,spawnRadius,spawnRadius) * 2);//Desenha a área de spawn possível dos NPCs
    }
}
