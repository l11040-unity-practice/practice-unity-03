using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData _data;

    public string GetInteractPromp()
    {
        string str = $"{_data.DisplayName}\n{_data.Description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.ItemData = _data;
        CharacterManager.Instance.Player.CallAddItem();
        Destroy(gameObject);
    }
}
