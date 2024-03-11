using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPad : InteractablePad
{
    [SerializeField]
    private float levelCost;
    protected override void PadAction(GameObject playerObj)
    {
        if(UiManager.instance.moneyF >= levelCost)
        {
            playerObj.GetComponent<PlayerController>().LevelUp(); //Aumenta o nível do player
            UiManager.instance.SubtractMoneyUI(levelCost); //Diminui o dinheiro e atualiza a UI

        }
    }
}
