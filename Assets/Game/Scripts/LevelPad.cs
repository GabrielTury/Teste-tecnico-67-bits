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
            playerObj.GetComponent<PlayerController>().LevelUp();
            UiManager.instance.SubtractMoneyUI(levelCost);

        }
    }
}
