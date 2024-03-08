using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public enum NpcState 
    {
        Idle,
        Walking,
        Ragdoll,
        Drop,
        Stacked
    }
    public NpcState currentState { get; private set; } = NpcState.Idle;

    #region Components
    private Rigidbody[] ragdollRbs;
    private Rigidbody mainRb;
    private Animator anim;
    private Collider col;
    #endregion

    private Transform stackPosition;

    private void Awake()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        mainRb = GetComponent<Rigidbody>();
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
                ChangeState(NpcState.Stacked);
                break;
            case NpcState.Stacked:
                currentState = NpcState.Stacked;
                anim.enabled = false;
                StartCoroutine(Stack());

                break;


        }
    }
    private void EnableRagdoll()
    {
        foreach(Rigidbody rb in ragdollRbs)
        {            
            rb.isKinematic = false;
        }
        col.enabled = false; // desabilita o mais collider
    }

    private IEnumerator Stack()
    {
        yield return new WaitForSeconds(3f);


        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        
        DisableRagdoll();

        while (currentState == NpcState.Stacked)
        {
            MoveRagdoll();
            //transform.position = stackPosition.position;
            //Debug.Log(stackPosition);
            yield return new WaitForEndOfFrame();
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = true;
        }
    }

    private void Punched(Transform stackPoint)
    {
        stackPosition = stackPoint;
        ChangeState(NpcState.Ragdoll);
    }

    private void MoveRagdoll()
    {
        ragdollRbs[1].position = stackPosition.position;
        //ragdollRbs[1].MovePosition(stackPosition.position);
    }
}
