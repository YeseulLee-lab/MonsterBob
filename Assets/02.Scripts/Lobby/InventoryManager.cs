using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    #endregion

    public Dictionary<LootData, int> lootInvenDic;
    [SerializeField] private int inventoryCapacity;

    public FieldInventory fieldInventory;

    private void Start()
    {
        lootInvenDic = new Dictionary<LootData, int>(inventoryCapacity);
    }

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

        fieldInventory.GetItem(loot);
    }
}