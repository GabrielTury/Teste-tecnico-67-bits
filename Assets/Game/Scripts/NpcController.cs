using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private enum NpcState
    {
        Idle,
        Walking,
        Ragdoll,
        Drop
    }

    private NpcState currentState = NpcState.Idle;
    #region Components
    private Rigidbody[] ragdollRbs;
    private Animator anim;
    #endregion

    private void Awake()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeState(NpcState newState)
    {
        StopAllCoroutines();

        switch(newState)
        {
            case NpcState.Idle:
                currentState = NpcState.Idle;
                anim.enabled = true;
                break;

            case NpcState.Walking:
                currentState = NpcState.Walking;
                anim.enabled = true;
                break;

            case NpcState.Drop:
                currentState = NpcState.Drop;
                anim.enabled = false;
                break;

            case NpcState.Ragdoll:
                currentState = NpcState.Ragdoll;
                anim.enabled = false;
                EnableRagdoll();
                break;


        }
    }

    private void EnableRagdoll()
    {
        foreach(Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = true;
        }
    }

    private void Punched()
    {
        ChangeState(NpcState.Ragdoll);
    }
}
