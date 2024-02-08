using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                InventoryManager find = GameObject.FindObjectOfType<InventoryManager>();
                if (find != null)
                {
                    _instance = find;
                }
                else
                    _instance = new GameObject().AddComponent<InventoryManager>();
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (FindObjectsOfType<InventoryManager>().Length != 1)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.HasKey("Energy"))
        {
            energyCount = PlayerPrefs.GetInt("Energy");
        }
        else
        {
            energyCount = 50;
        }

        if (PlayerPrefs.HasKey("Crystal"))
        {
            crystalCount = PlayerPrefs.GetInt("Crystal");
        }
        else
        {
            crystalCount = 50;
        }

    }
    #endregion

    [Header("Inventory")]
    public Dictionary<LootData, int> lootInvenDic;
    [SerializeField] private int inventoryCapacity;

    public FieldInventory fieldInventory;

    [Header("재화")]
    private int energyCount = 50;
    private int crystalCount = 50;

    public int EnergyCount
    {
        get { return energyCount; }
        set
        {
            PlayerPrefs.SetInt("Energy", energyCount);
        }
    }

    public int CrystalCount
    {
        get { return crystalCount; }
        set
        {
            PlayerPrefs.SetInt("Crystal", energyCount);
        }
    }

    private void Start()
    {
        lootInvenDic = new Dictionary<LootData, int>(inventoryCapacity);
    }

    #region Inventory

    public void GetLoots(LootData loot)
    {
        if (lootInvenDic.Count >= inventoryCapacity)
        {
            return;
        }

        if (lootInvenDic.ContainsKey(loot))
        {
            lootInvenDic[loot]++;
        }
        else
        {
            lootInvenDic.Add(loot, 1);
        }

        if(fieldInventory != null)
            fieldInventory.SetItem(loot);
    }

    public void UseLoots(LootData loot)
    {
        if (lootInvenDic.ContainsKey(loot))
        {
            lootInvenDic[loot] --;
        }
    }

    public void RemoveLoots(LootData loot)
    {
        if (lootInvenDic.ContainsKey(loot))
        {
            lootInvenDic.Remove(loot);
        }
    }
    #endregion

    #region 재화
    public int SetEnergy(int amount)
    {
        if (amount < 0)
        {
            if(energyCount > amount)
                energyCount += amount;
        }
        else
        {
            energyCount += amount;
        }
        return energyCount;
    }

    public int SetCrystal(int amount)
    {
        crystalCount += amount;
        return crystalCount;
    }
    #endregion
}