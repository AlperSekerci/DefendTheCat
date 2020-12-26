using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }
    public int startingGold = 1000;
    public int unitCost = 10;
    public int unitExtraCost = 2;
    private int unitPurchaseCt = 0;
    public Text goldInfoText;
    public Text unitCostText;
    private const string UNIT_COST_LABEL = "Unit Cost: ";

    private int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            goldInfoText.text = "Gold: " + value;
        }
    }
    
    private void Start()
    {
        Instance = this;
        Gold = startingGold;
    }

    public bool CanBuyUnit()
    {   
        return GetUnitCost() <= Gold;
    }

    public int GetUnitCost()
    {
        return unitCost + unitPurchaseCt * unitExtraCost;
    }

    public void Reset()
    {
        Gold = startingGold;
        unitPurchaseCt = 0;
        UpdateUnitCostText();
    }

    public void WavePrepStart()
    {
        unitPurchaseCt = 0;
    }

    public void PurchaseUnit()
    {
        if (!CanBuyUnit()) return;
        Gold -= GetUnitCost();
        unitPurchaseCt++;
        UpdateUnitCostText();
    }

    private void UpdateUnitCostText()
    {
        unitCostText.text = UNIT_COST_LABEL + GetUnitCost();
    }
}