using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    //public List<GameObject> stackedNpcs;

    public Stack<GameObject> stackedNpcs;

    private Transform[] stackPoints;

    #region Stack Variables
    [SerializeField]
    private int maxStack;
    [SerializeField]
    private int stackLimit;

    private int stackCount = 0;
    #endregion

    [SerializeField]
    private GameObject stackPointTemplate;

    private PlayerController playerController;

    [SerializeField,Tooltip("Constante elastica, quanto maior mais dura a ligação entres os corpos")]
    private float k;


    private void Awake()
    {
        stackPoints = new Transform[maxStack];
        stackedNpcs = new Stack<GameObject>();
    }
    private void Start()
    {
        playerController = stackPointTemplate.GetComponentInParent<PlayerController>();

        //Instancia os objetos vazios que vão servir de base para o movimento da pilha
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
        //Debug.Log(stackCount);
        int i = 0;
        foreach (Transform t in stackPoints)
        {
            if(t == stackPoints[0]) //Define a posição e rotação do primeiro da pilha
            {
                Vector3 velocity = -k * (t.position - stackPointTemplate.transform.position);
                velocity.y = 0;

                t.position = t.position + velocity * Time.deltaTime;
                t.rotation = Quaternion.Euler(90,0,0);
            }
            else
            {
                Vector3 velocity = -k * (t.position - stackPoints[i - 1].position); //Lei de Hooke para liga um objeto com o anterior como se fosse uma mola/elástico
                velocity.y = 0; //faz com que não se movimente no eixo Y cpela lei de hooke

                t.position = t.position + velocity * Time.deltaTime; //Soma a posição com a direção dada por hooke x tempo criando a velocidade de movimento

                t.LookAt(stackPointTemplate.transform); //rotaciona a pilha para estar sempre olhando para a parte de baixo e dar impressão de inércia
            }
            i++;
        }
    }

    public bool CheckIfAvailable()
    {
        if (stackCount <= stackLimit)
        {
           
            return true;
        }

        return false;
    }


    /// <summary>
    /// Adiciona NPC para a lista de NPCs stackados e os coloca em ordem removendo os nulos
    /// </summary>
    /// <param name="npc"></param>
    public void AddNpcToStack(GameObject npc)
    {
        stackedNpcs.Push(npc);
        
    }
    /// <summary>
    /// Retorna o próximo ponto a ser preenchido caso exista
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Aumenta os pontos possíveis
    /// </summary>
    /// <param name="increaseAmount"></param>
    public void IncreaseStackLimit(int increaseAmount)
    {
        stackLimit += increaseAmount;

        if(stackLimit > maxStack)
        {
            stackLimit = maxStack;
        }
    }
    /// <summary>
    /// libera aquele objeto da pilha
    /// </summary>
    /// <param name="releasePosition"></param>
    /// <returns></returns>
    public bool ReleaseStack(Vector3 releasePosition)
    {
        if(stackCount > 0)
        {
            //Debug.Log("Stack Count" + stackCount);
            //Debug.Log("List: " + stackedNpcs.Count);
            stackCount--;
            NpcController npcScript = stackedNpcs.Pop().GetComponent<NpcController>();

            //stackedNpcs.Pop();
            


            npcScript.SmoothMoveRagdoll(releasePosition);
            return true;
        }
        return false;
    }
}
