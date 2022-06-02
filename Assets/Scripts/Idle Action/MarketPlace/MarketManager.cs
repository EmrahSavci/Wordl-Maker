using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public List<AreData> areDatas = new List<AreData>();
    [SerializeField]
    private List<PurchasableArea> purchasableAreas = new List<PurchasableArea>();

    public static MarketManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        purchasableAreas = GetComponentsInChildren<PurchasableArea>().ToList();
    }
    private void Start()
    {
        var data = DataExtension.RunTimeGetData();
        areDatas = data.areDatas;   
        foreach (var purchasableArea in purchasableAreas)
        {
            var areData = areDatas.FirstOrDefault(x => x.AreaName == purchasableArea.AreaName);
            if (areData != null) {
                purchasableArea.Setup(areData.Bought,areData.Deposit);
            }
            else {
                purchasableArea.Setup(false,0);
            }
        }
    }

    private void OnDisable()
    {
        Instance = null;
    }
    
    public void SendData()
    {
        areDatas.Clear();
        foreach (var purchasableArea in purchasableAreas)
        {
            areDatas.Add(new AreData(purchasableArea.AreaName, purchasableArea.DepositPrice, purchasableArea.Bought));
        }

        DataExtension.RunTimeGetData().areDatas = areDatas;
        DataExtension.SaveData(DataExtension.RunTimeGetData());
    }

}

public static class MarketExtension
{   
    public static void SendData(this MarketManager marketManager)
    {
        marketManager.SendData();
    }
}

public partial class Data
{
    public List<AreData> areDatas = new List<AreData>();
}
[System.Serializable]
public class AreData
{
    public string AreaName;
    public bool Bought;
    public int Deposit;

    public AreData(string areaName, int deposit, bool bought)
    {
        AreaName = areaName;
        Bought = bought;
        Deposit = deposit;
    }
}
