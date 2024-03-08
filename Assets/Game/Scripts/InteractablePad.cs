using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractablePad : MonoBehaviour
{
    private float timeCounter;

    [SerializeField]
    private float maxTimer;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StackManager stack = other.gameObject.GetComponent<StackManager>();
            
            if(timeCounter >= maxTimer)
            {

                timeCounter = 0;
            }
            timeCounter += Time.deltaTime;
        }
    }
}
