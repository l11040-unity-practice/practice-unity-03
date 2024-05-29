using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
}

public class NPC : MonoBehaviour, IDamagable
{
    [Header("Stat")]
    [SerializeField] private int _health;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private ItemData[] _dropDeath;

    [Header("AI")]
    private NavMeshAgent _agent;
    private AIState _aiState;
    [SerializeField] private float _detectDistance;

    [Header("Wandering")]
    [SerializeField] private float _minWanderDistance;
    [SerializeField] private float _maxWanderDistance;
    [SerializeField] private float _minWanderWaitTime;
    [SerializeField] private float _maxWanderWaitTime;

    [Header("Combat")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRate;
    [SerializeField] private float _attackDistance;
    private float _lastAttackTime;

    [SerializeField] private float _fieldOfView = 120f;
    private float _playerDistance;
    private Animator _animator;
    private SkinnedMeshRenderer[] _meshRenderers;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        _animator.SetBool("IsMoving", _aiState != AIState.Idle);

        switch (_aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState state)
    {
        _aiState = state;
        switch (_aiState)
        {
            case AIState.Idle:
                _agent.speed = _walkSpeed;
                _agent.isStopped = true;
                break;
            case AIState.Wandering:
                _agent.speed = _walkSpeed;
                _agent.isStopped = false;
                break;
            case AIState.Attacking:
                _agent.speed = _runSpeed;
                _agent.isStopped = false;
                break;
        }
        _animator.speed = _agent.speed / _walkSpeed;
    }

    private void PassiveUpdate()
    {
        if (_aiState == AIState.Wandering && _agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(_minWanderWaitTime, _maxWanderWaitTime));
        }

        if (_playerDistance < _detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }
    private void WanderToNewLocation()
    {
        if (_aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        _agent.SetDestination(GetWanderLocation());
    }
    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(_minWanderDistance, _maxWanderDistance)), out hit, _maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < _detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(_minWanderDistance, _maxWanderDistance)), out hit, _maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
            {
                break;
            }
        }
        return hit.position;
    }
    private void AttackingUpdate()
    {
        if (_playerDistance < _attackDistance && IsPlayerInFieldOfView())
        {
            _agent.isStopped = true;
            if (Time.time - _lastAttackTime > _attackRate)
            {
                _lastAttackTime = Time.time;
                CharacterManager.Instance.Player.Controller.GetComponent<IDamagable>().TakePhysicalDamage(_damage);
                _animator.speed = 1;
                _animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (_playerDistance < _detectDistance)
            {
                _agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (_agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    _agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    _agent.SetDestination(transform.position);
                    _agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                _agent.SetDestination(transform.position);
                _agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < _fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        _health -= _damage;
        if (_health <= 0)
        {
            Die();
        }
        StartCoroutine(DamageFlash());
    }

    private void Die()
    {
        for (int i = 0; i < _dropDeath.Length; i++)
        {
            Instantiate(_dropDeath[i].DropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material.color = Color.white;
        }
    }
}
