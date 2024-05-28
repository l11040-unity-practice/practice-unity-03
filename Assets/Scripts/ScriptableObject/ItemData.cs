using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

public enum ConsumableType
{
    Health,
    Hunger,
}
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType Type;
    public float Value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string DisplayName;
    public string Description;
    public ItemType Type;
    public Sprite Icon;
    public GameObject DropPrefab;

    [Header("Stacking")]
    public bool CanStack;
    public int MaxStackAmount;

    [Header("Consumables")]
    public ItemDataConsumable[] Consumables;
}
