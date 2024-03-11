using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractablePad : MonoBehaviour
{
    private float timeCounter;

    [SerializeField]
    private float maxTimer; //Tempo q o player tem q ficar em cima para interagir

    protected abstract void PadAction(GameObject playerObj); //função abstrata que vai conter o funcionamento das diferentes bases


    private void OnTriggerStay(Collider other)
    {
        /*Debug.Log("Trigger Stay");*/
        if (other.CompareTag("Player"))
        {

            GameObject playerObj = other.gameObject;

            if(timeCounter >= maxTimer)
            {


                timeCounter = 0;

                PadAction(playerObj);
            }
            timeCounter += Time.deltaTime;
        }
    }
}
