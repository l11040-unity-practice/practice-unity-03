using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [HideInInspector] public UIInventory Inventory;
    [HideInInspector] public ItemData Item;

    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Outline _outline;

    [HideInInspector] public int Index;
    [HideInInspector] public int Quantity;
    [HideInInspector] public bool IsEquipped;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }
    private void OnEnable()
    {
        _outline.enabled = IsEquipped;
    }
    public void Set()
    {
        _icon.gameObject.SetActive(true);
        _icon.sprite = Item.Icon;
        _quantityText.text = Quantity > 1 ? Quantity.ToString() : string.Empty;

        if (_outline != null)
        {
            _outline.enabled = IsEquipped;
        }
    }

    public void Clear()
    {
        Item = null;
        _icon.gameObject.SetActive(false);
        _quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        Inventory.SelectItem(Index);
    }
}
