using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookData", menuName = "ScriptableObject/CookData", order = int.MaxValue)]
public class CookData : ScriptableObject
{
    public int uid;
    public Sprite sprite;
    public string cookName;
    public string desc;
    public float cookTime;
}
