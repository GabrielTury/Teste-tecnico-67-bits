using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public GameObject[] stackedNpcs;

    private Transform[] stackPoints;

    [SerializeField]
    private int maxStack;
    [SerializeField]
    private int stackLimit;

    private int stackCount = 0;

    [SerializeField]
    private GameObject stackPointTemplate;


    private void Awake()
    {
        stackPoints = new Transform[maxStack];
    }
    private void Start()
    {
        int i = 0;
        foreach (Transform t in stackPoints)
        {
            GameObject go = Instantiate(stackPointTemplate, gameObject.transform);


            stackPoints[i] = go.transform;
            stackPoints[i].position = stackPointTemplate.transform.position + new Vector3(0, i, 0);

            i++;
        }
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
}
