using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerCondition Condition;
    private PlayerController _controller;
    public ItemData ItemData;
    public event Action OnAddItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        _controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
    }

    public void CallAddItem()
    {
        OnAddItem?.Invoke();
    }
}