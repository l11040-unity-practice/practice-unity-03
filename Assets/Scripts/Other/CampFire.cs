using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    [SerializeField] private int _demage;
    [SerializeField] private float _demageRate;
    List<IDamagable> things = new List<IDamagable>();
    private void Start()
    {
        InvokeRepeating("DealDamage", 0, _demageRate);
    }
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(_demage);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}