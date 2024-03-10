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

    [SerializeField]
    private float k;


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
        int i = 0;
        foreach (Transform t in stackPoints)
        {
            if(t == stackPoints[0])
            {
                t.position = stackPointTemplate.transform.position;
            }
            else
            {
                Vector3 velocity = -k * (t.position - stackPoints[i - 1].position);
                velocity.y = 0;

                t.position = t.position + velocity * Time.deltaTime;
            }
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
