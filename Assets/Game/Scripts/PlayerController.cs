using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), (typeof(Collider)))]
public class PlayerController : MonoBehaviour
{
    #region Components
    private Rigidbody rb;

    private Collider col;

    private Animator anim;

    [SerializeField]
    private Transform stackPoint;
    #endregion

    #region Inputs
    private Inputs inputs;
    #endregion

    #region Movement
    private Vector2 joystickDirection;
    private Vector3 moveDirection;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    #endregion

    private void Awake()
    {
        inputs = new Inputs();
        inputs.MainMap.Enable();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        inputs.MainMap.Move.performed += Move_performed;
        inputs.MainMap.Move.canceled += Move_canceled;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        moveDirection = Vector3.zero;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        joystickDirection = obj.ReadValue<Vector2>();

        moveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);
    }


    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDirection * moveSpeed;
        anim.SetFloat("speed", rb.velocity.magnitude);

        if(moveDirection != Vector3.zero)
        {          
            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {  
        //Troca o estado do Npc e ativa a animação de soco (Tentar otimizar)
        if(other.CompareTag("NPC"))
        {                      
            NpcController npcScript = other.GetComponent<NpcController>();
            if(npcScript != null)
            {
                if (npcScript.currentState != NpcController.NpcState.Ragdoll)
                {
                    anim.SetTrigger("punch");

                    other.SendMessage("Punched",stackPoint, SendMessageOptions.DontRequireReceiver);
                }
            }

            
        }
    }
}
