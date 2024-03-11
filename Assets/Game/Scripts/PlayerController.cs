using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), (typeof(Collider)), (typeof(StackManager)))]
public class PlayerController : MonoBehaviour
{
    #region Components
    private Rigidbody rb;

    private Collider col;

    private Animator anim;

    #endregion


    private StackManager stackManager;

    #region Level Variables
    private int level;
    #endregion
    #region Inputs
    private Inputs inputs;
    #endregion

    #region Movement Variables
    private Vector2 joystickDirection;
    public Vector3 moveDirection;

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
        stackManager = GetComponent<StackManager>();

        inputs.MainMap.Move.performed += Move_performed;
        inputs.MainMap.Move.canceled += Move_canceled;
    }

    #region Input Delegates
    /// <summary>
    /// Chamado quando o joystick para de ser mexido
    /// </summary>
    /// <param name="obj"></param>
    private void Move_canceled(InputAction.CallbackContext obj)
    {
        moveDirection = Vector3.zero;
    }

    /// <summary>
    /// Chamado enquanto o joystick estiver sendo usado
    /// </summary>
    /// <param name="obj"></param>
    private void Move_performed(InputAction.CallbackContext obj)
    {
        joystickDirection = obj.ReadValue<Vector2>();

        moveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);
    }
    #endregion

    #region Updates
    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed; //movimento simples
    }
    // Update is called once per frame
    void Update()
    {
        
        anim.SetFloat("speed", rb.velocity.magnitude); //atualiza o animator

        if(moveDirection != Vector3.zero)
        {          
            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime); //faz virar na direção do movimento
        }
    }
    #endregion
    /// <summary>
    /// Cuida da mudança de Cor quando sobe de Nível e chama o StackManager para aumenta o número de objetos permitidos
    /// </summary>
    public void LevelUp()
    {
        if (level < 5)
        {

            level++;
            stackManager.IncreaseStackLimit(3);
            UiManager.instance.AddLevelUI(1);

            Material mainMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
            //Change Color
            switch (level)
            {
                case 1:
                    mainMat.color = Color.blue;
                    break;
                case 2:
                    mainMat.color = Color.red;
                    break;
                case 3:
                    mainMat.color = Color.cyan;
                    break;
                case 4:
                    mainMat.color = Color.magenta;
                    break;
            }
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
                    anim.SetTrigger("punch"); // ativa o animator

                    if (stackManager.CheckIfAvailable())
                    {
                        
                        stackManager.AddNpcToStack(other.gameObject);

                    }

                    npcScript.Punched(stackManager.GetNextStackPoint());
                    //other.SendMessage("Punched",stackManager.GetNextStackPoint(), SendMessageOptions.DontRequireReceiver);

                }
            }

            
        }
    }
}
