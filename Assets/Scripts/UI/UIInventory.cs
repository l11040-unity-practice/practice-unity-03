using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private ItemSlot[] _slots;
    [SerializeField] private GameObject _inventoryWindow;
    [SerializeField] private Transform _slotPanel;

    [Header("Select Item")]
    [SerializeField] private TextMeshProUGUI _selectedItemName;
    [SerializeField] private TextMeshProUGUI _selectedItemDescription;
    [SerializeField] private TextMeshProUGUI _selectedStatName;
    [SerializeField] private TextMeshProUGUI _selectedStatValue;
    [SerializeField] private GameObject _useButton;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _unequipButton;
    [SerializeField] private GameObject _dropButton;

    private PlayerController _controller;
    private PlayerCondition _condition;
    private Transform _dropPos;
    private ItemData _selectedItem;
    private int _selectedItemIndex;
    private int _curEquipIndex;

    void Start()
    {
        _controller = CharacterManager.Instance.Player.Controller;
        _condition = CharacterManager.Instance.Player.Condition;
        _dropPos = CharacterManager.Instance.Player.DropPos;

        _controller.OnInventoryEvent += Toggle;
        CharacterManager.Instance.Player.OnAddItemEvent += AddItem;

        _inventoryWindow.SetActive(false);
        _slots = new ItemSlot[_slotPanel.childCount];
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = _slotPanel.GetChild(i).GetComponent<ItemSlot>();
            _slots[i].Index = i;
            _slots[i].Inventory = this;
        }
        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        _selectedItemName.text = string.Empty;
        _selectedItemDescription.text = string.Empty;
        _selectedStatName.text = string.Empty;
        _selectedStatValue.text = string.Empty;

        _useButton.SetActive(false);
        _equipButton.SetActive(false);
        _unequipButton.SetActive(false);
        _dropButton.SetActive(false);
    }

    private void Toggle()
    {
        if (IsOpen())
        {
            _inventoryWindow.SetActive(false);
        }
        else
        {
            _inventoryWindow.SetActive(true);
        }
    }

    private bool IsOpen()
    {
        return _inventoryWindow.activeInHierarchy;
    }

    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.ItemData;

        if (data.CanStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.Quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.ItemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySLot();
        if (emptySlot != null)
        {
            emptySlot.Item = data;
            emptySlot.Quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.ItemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.ItemData = null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Item != null)
            {
                _slots[i].Set();
            }
            else
            {
                _slots[i].Clear();
            }
        }
    }

    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Item == data && _slots[i].Quantity < data.MaxStackAmount)
            {
                return _slots[i];
            }
        }
        return null;
    }

    private ItemSlot GetEmptySLot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Item == null)
            {
                return _slots[i];
            }
        }
        return null;
    }

    private void ThrowItem(ItemData data)
    {
        Instantiate(data.DropPrefab, _dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (_slots[index].Item == null) return;

        _selectedItem = _slots[index].Item;
        _selectedItemIndex = index;

        _selectedItemName.text = _selectedItem.DisplayName;
        _selectedItemDescription.text = _selectedItem.Description;

        _selectedStatName.text = string.Empty;
        _selectedStatValue.text = string.Empty;

        for (int i = 0; i < _selectedItem.Consumables.Length; i++)
        {
            _selectedStatName.text += _selectedItem.Consumables[i].Type.ToString() + "\n";
            _selectedStatValue.text += _selectedItem.Consumables[i].Value.ToString() + "\n";
        }

        _useButton.SetActive(_selectedItem.Type == ItemType.Consumable);
        _equipButton.SetActive(_selectedItem.Type == ItemType.Equipable && !_slots[index].IsEquipped);
        _unequipButton.SetActive(_selectedItem.Type == ItemType.Equipable && _slots[index].IsEquipped);

        _dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (_selectedItem.Type == ItemType.Consumable)
        {
            for (int i = 0; i < _selectedItem.Consumables.Length; i++)
            {
                switch (_selectedItem.Consumables[i].Type)
                {
                    case ConsumableType.Health:
                        _condition.Heal(_selectedItem.Consumables[i].Value);
                        break;
                    case ConsumableType.Hunger:
                        _condition.Eat(_selectedItem.Consumables[i].Value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }
    public void OnDropButton()
    {
        ThrowItem(_selectedItem);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        _slots[_selectedItemIndex].Quantity--;

        if (_slots[_selectedItemIndex].Quantity <= 0)
        {
            _selectedItem = null;
            _slots[_selectedItemIndex].Item = null;
            _selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (_slots[_curEquipIndex].IsEquipped)
        {
            UnEquip(_curEquipIndex);
        }
        _slots[_selectedItemIndex].IsEquipped = true;
        _curEquipIndex = _selectedItemIndex;
        CharacterManager.Instance.Player.Equipment.NewEquip(_selectedItem);
        UpdateUI();
        SelectItem(_selectedItemIndex);
    }
    public void OnUnEquipButton()
    {
        UnEquip(_selectedItemIndex);
    }

    private void UnEquip(int index)
    {
        _slots[index].IsEquipped = false;
        CharacterManager.Instance.Player.Equipment.UnEquip();
        UpdateUI();

        if (_selectedItemIndex == index)
        {
            SelectItem(_selectedItemIndex);
        }
    }

}
