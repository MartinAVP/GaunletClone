using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Players", menuName = "New Player")]
public class Players : ScriptableObject
{
    [HideInInspector]public int id;
    [Header("Information")]
    public string name;
    public int damage;
    public int health;
    public playerType type;

    public List<inventoryItems> inventory;

    public GameObject prefab;
    public Color UIColor;

    [System.Serializable]
    public struct inventoryItems
    {
        public itemTypes type;
        public int quantity;
    }
}
