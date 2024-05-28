using UnityEngine;

public class EquipTool : Equip
{
  [SerializeField] private float _attackRate;
  [SerializeField] private float _attackDistance;
  private bool _isAttacking;

  [Header("Resource Gathering")]
  [SerializeField] private bool _doesGatherResources;

  [Header("Combat")]
  [SerializeField] private bool _doesDealDamage;
  [SerializeField] private int _damage;

  private Animator _animator;

  private void Start()
  {
    _animator = GetComponent<Animator>();
  }

  public override void OnAttack()
  {
    if (!_isAttacking)
    {
      _isAttacking = true;
      _animator.SetTrigger("Attack");
      Invoke("OnCanAttack", _attackRate);
    }
  }

  private void OnCanAttack()
  {
    _isAttacking = false;
  }
}