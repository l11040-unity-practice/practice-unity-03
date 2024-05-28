using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ItemData _itemToGive;
    [SerializeField] private int _quantityPerHit = 1;
    [SerializeField] private int _capacity;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < _quantityPerHit; i++)
        {
            if (_capacity <= 0) break;
            _capacity -= 1;
            Instantiate(_itemToGive.DropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
}
