using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerCondition Condition;
    private PlayerController _controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        _controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
    }
}