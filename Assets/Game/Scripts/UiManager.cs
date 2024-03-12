using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    #region Singleton
    public static UiManager instance;
    #endregion

    public float moneyF;
    private int levelI;

    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private TextMeshProUGUI level;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Adiciona dinheiro e Atualiza a UI
    /// </summary>
    /// <param name="addValue"></param>
    public void AddMoneyUI(float addValue)
    {
        moneyF += addValue;
        string newValue = moneyF.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));

        //Update Value
        money.text = newValue;
    }
    /// <summary>
    /// Subtrai dinheiro e Atualiza a UI
    /// </summary>
    /// <param name="subtractValue"></param>
    public void SubtractMoneyUI(float subtractValue)
    {
        moneyF -= subtractValue;
        string newValue = moneyF.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));

        //Update Value
        money.text = newValue;
    }
    /// <summary>
    /// Atualiza a UI com o nível Atual
    /// </summary>
    /// <param name="addValue"></param>
    public void AddLevelUI(int addValue)
    {
        levelI += addValue;
        string newValue = levelI.ToString();

        level.text = "LV: " + newValue;
    }
}
