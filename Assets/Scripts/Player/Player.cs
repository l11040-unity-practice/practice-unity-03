using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerCondition Condition;
    [HideInInspector] public PlayerController Controller;
    [HideInInspector] public PlayerEquipment Equipment;
    [HideInInspector] public ItemData ItemData;
    [HideInInspector] public event Action OnAddItemEvent;
    public Transform DropPos;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
        Equipment = GetComponent<PlayerEquipment>();
    }

    public void CallAddItem()
    {
        OnAddItemEvent?.Invoke();
    }
}