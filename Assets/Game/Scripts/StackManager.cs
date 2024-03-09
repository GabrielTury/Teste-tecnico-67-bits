using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public List<GameObject> stackedNpcs;

    private Transform[] stackPoints;

    [SerializeField]
    private int maxStack;
    [SerializeField]
    private int stackLimit;

    private int stackCount = 0;

    [SerializeField]
    private GameObject stackPointTemplate;

    private PlayerController playerController;


    private void Awake()
    {
        stackPoints = new Transform[maxStack];
        stackedNpcs = new List<GameObject>();
    }
    private void Start()
    {
        playerController = stackPointTemplate.GetComponentInParent<PlayerController>();

        int i = 0;
        foreach (Transform t in stackPoints)
        {
            GameObject go = Instantiate(stackPointTemplate);


            stackPoints[i] = go.transform;
            stackPoints[i].position = stackPointTemplate.transform.position + new Vector3(0, i, 0);

            i++;
        }
    }

    private void LateUpdate()
    {
        float i = 1;

        foreach(Transform t in stackPoints) 
        {
            float speed = 2f- i/25f; //Tentar com Corotina
            t.position = Vector3.MoveTowards(t.position, stackPointTemplate.transform.position + new Vector3(0, i - 1, 0), speed * Time.deltaTime);
            Debug.Log("N - "+ i + ": " + speed);

            i++;
        }
    }

    public void AddNpcToStack(GameObject npc)
    {
        stackedNpcs.Add(npc);
    }

    public Transform GetNextStackPoint()
    {
        if(stackCount <= stackLimit)
        {
            stackCount++;

            return stackPoints[stackCount - 1];
        }
        else
        {
            return null;
        }
    }

    public void IncreaseStackLimit(int increaseAmount)
    {
        stackLimit += increaseAmount;

        if(stackLimit > maxStack)
        {
            stackLimit = maxStack;
        }
    }

    public bool ReleaseStack(Vector3 releasePosition)
    {
        if(stackCount > 0)
        {
            //Debug.Log("Stack Count" + stackCount);
            //Debug.Log("List: " + stackedNpcs.Count);
            stackCount--;
            NpcController npcScript = stackedNpcs[stackCount].GetComponent<NpcController>();

            stackedNpcs.RemoveAt(stackCount);

            

            npcScript.SmoothMoveRagdoll(releasePosition);
            return true;
        }
        return false;
    }
}
