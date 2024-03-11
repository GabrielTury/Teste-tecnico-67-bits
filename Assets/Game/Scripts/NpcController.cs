using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    #region State Machine Enum
    public enum NpcState 
    {
        Idle,
        Ragdoll,
        Drop,
        Stacked
    }
    public NpcState currentState { get; private set; } = NpcState.Idle;
    #endregion

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

    #region State Machines
    /// <summary>
    /// Altera o Estado do NPC e executa o que necessita
    /// </summary>
    /// <param name="newState"></param>
    private void ChangeState(NpcState newState)
    {
        StopAllCoroutines();

        switch(newState)
        {
            case NpcState.Idle:
                currentState = NpcState.Idle;
                anim.enabled = true;
                break;

            case NpcState.Drop:
                currentState = NpcState.Drop;
                EnableRagdoll();
                StartCoroutine(Drop());
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
    #endregion

    #region Ragdoll Change
    /// <summary>
    /// Habilita o ragdoll
    /// </summary>
    private void EnableRagdoll()
    {
        foreach(Rigidbody rb in ragdollRbs)
        {            
            rb.isKinematic = false;
        }
        col.enabled = false; // desabilita o mais collider
    }
    /// <summary>
    /// Desabilita o ragdoll
    /// </summary>
    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = true;
        }
    }
    #endregion

    #region CoRoutines
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
            MoveRagdoll(stackPosition.position);
            ragdollRbs[1].rotation = stackPosition.rotation;

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(2f);
        NpcSpawner.instance.RemoveNpc();
        Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// Chamado quando recebe um soco
    /// </summary>
    /// <param name="stackPoint"></param>
    public void Punched(Transform stackPoint)
    {
        if(stackPoint != null)
        {
            stackPosition = stackPoint;
            ChangeState(NpcState.Ragdoll);

        }
        else
        {
            ChangeState(NpcState.Drop);
        }

    }

    #region Move Functions
    private void MoveRagdoll(Vector3 destination)
    {
        ragdollRbs[1].position = destination;
        
    }

    public void SmoothMoveRagdoll(Vector3 destination)
    {
        ChangeState(NpcState.Drop);

        ragdollRbs[1].AddForce(Vector3.up * 3, ForceMode.Impulse);

        ragdollRbs[1].MovePosition(destination);

        
    }
    #endregion

}
