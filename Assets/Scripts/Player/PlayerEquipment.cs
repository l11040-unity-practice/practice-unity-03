using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    [HideInInspector] public Equip CurEquip;
    [SerializeField] private Transform _equipParent;

    private PlayerController _controller;
    private PlayerCondition _condition;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _condition = GetComponent<PlayerCondition>();
    }

    public void NewEquip(ItemData data)
    {
        UnEquip();
        CurEquip = Instantiate(data.EquipPrefab, _equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (CurEquip != null)
        {
            Destroy(CurEquip.gameObject);
            CurEquip = null;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CurEquip != null && _controller.CanLook)
        {
            CurEquip.OnAttack();
        }
    }
}
