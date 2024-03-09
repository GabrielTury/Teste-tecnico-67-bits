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
        //Lógica para jogar o NPC aqui
        if (stackManager.ReleaseStack(gameObject.transform.position))
        {
            UiManager.instance.AddMoneyUI(moneyToAdd);
        }
    }

}
