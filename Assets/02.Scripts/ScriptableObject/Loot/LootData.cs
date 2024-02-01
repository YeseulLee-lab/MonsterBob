using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootData", menuName = "ScriptableObject/LootData", order = int.MaxValue)]
public class LootData : ScriptableObject
{
    public int uid;
    public Sprite sprite;
    public string lootName;
    public string desc;
    public int dropChance;
}
