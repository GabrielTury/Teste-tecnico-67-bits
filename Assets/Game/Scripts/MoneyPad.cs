using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPad : InteractablePad
{

    [SerializeField, Range(0f, 20f)]
    private float moneyToAdd;
    protected override void PadAction(GameObject playerObj)
    {
        StackManager stackManager = playerObj.GetComponent<StackManager>();
        //L�gica para jogar o NPC aqui
        if (stackManager.ReleaseStack(gameObject.transform.position))//Libera o corpo da pilha e serve de boolean para checar se foi solto um corpo ou n
        {
            UiManager.instance.AddMoneyUI(moneyToAdd); //adiciona dinheiro e atualiza a UI
        }
    }

}
