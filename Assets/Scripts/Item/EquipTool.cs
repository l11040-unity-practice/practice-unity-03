using UnityEngine;

public class EquipTool : Equip
{
  [SerializeField] private float _attackRate;
  [SerializeField] private float _attackDistance;
  [SerializeField] private float _useStamina;
  private bool _isAttacking;

  [Header("Resource Gathering")]
  [SerializeField] private bool _doesGatherResources;

  [Header("Combat")]
  [SerializeField] private bool _doesDealDamage;
  [SerializeField] private int _damage;

  private Animator _animator;
  private Camera _camera;

  private void Start()
  {
    _animator = GetComponent<Animator>();
    _camera = Camera.main;
  }

  public override void OnAttack()
  {
    if (!_isAttacking)
    {
      if (CharacterManager.Instance.Player.Condition.UseStamina(_useStamina))
      {
        _isAttacking = true;
        _animator.SetTrigger("Attack");
        Invoke("OnCanAttack", _attackRate);
      }
    }
  }

  private void OnCanAttack()
  {
    _isAttacking = false;
  }

  public void OnHit()
  {
    Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    if (Physics.Raycast(ray, out RaycastHit hit, _attackDistance))
    {
      if (_doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
      {
        resource.Gather(hit.point, hit.normal);
      }
    }
  }
}