using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavSystem : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField] BoxCollider _boxCol;
    NavMeshAgent _navAgent;
    Rigidbody _rigid;

    public float pathEndThreshold = 0.1f;
    private bool hasPath = false;
    private bool _isLooking = false;

    public enum CurrentBehaviour
    {
        Wait,
        Chase,
        Attack
    }

    [SerializeField] CurrentBehaviour _currentBehaviour;

    public CurrentBehaviour CurrentEnemyState { get => _currentBehaviour; set => _currentBehaviour = value; }
    public BoxCollider BoxCol { get => _boxCol; set => _boxCol = value; }


    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
        _currentBehaviour = CurrentBehaviour.Wait;
        StartCoroutine(WaitForChase());
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentBehaviour == CurrentBehaviour.Chase)
        {
            SelectBehavior(CurrentBehaviour.Chase);
        }
        else
        {
            if(_isLooking == true)
            {
                transform.LookAt(_target.transform);
            }
        }
    }

    void SelectBehavior(CurrentBehaviour _state)
    {
        _currentBehaviour = _state;
        var _distToTarget = Vector3.Distance(transform.position, _target.transform.position);

        switch (_currentBehaviour)
        {
            case CurrentBehaviour.Wait:
                _navAgent.isStopped = true;

                break;
            case CurrentBehaviour.Chase:
                _navAgent.isStopped = false;

                if (ReachToTarget())
                {
                    SelectBehavior(CurrentBehaviour.Attack);
                }
                else
                {
                    _navAgent.SetDestination(_target.transform.position);
                }

                break;
            case CurrentBehaviour.Attack:
                if (ReachToTarget())
                {
                    _navAgent.isStopped = true;
                    StartCoroutine(AttackTarget());

                }
                else
                {
                    SelectBehavior(CurrentBehaviour.Chase);
                }

                break;
        }
    }

    private bool ReachToTarget()
    {
        var _distToTarget = Vector3.Distance(transform.position, _target.transform.position);
        if (_distToTarget <= _navAgent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator WaitForChase()
    {
        yield return new WaitForSeconds(2f);
        SelectBehavior(CurrentBehaviour.Chase);
        //_navAgent.SetDestination(_target.transform.position);
    }

    IEnumerator AttackTarget()
    {
        print("박치기 준비");
        _isLooking = true;

        yield return new WaitForSeconds(1f);
        print("조준");
        var _reactDir = _target.transform.position - transform.position;
        _reactDir = _reactDir.normalized;
        _isLooking = false;

        yield return new WaitForSeconds(0.1f);
        print("박치기");
        _rigid.AddForce(_reactDir * 20f, ForceMode.Impulse);
        BoxCol.enabled = true;

        yield return new WaitForSeconds(0.5f);
        _rigid.velocity = Vector3.zero;
        BoxCol.enabled = false;

        yield return new WaitForSeconds(2f);
        print("재설정");
        if (ReachToTarget())
        {
            SelectBehavior(CurrentBehaviour.Attack);

        }
        else
        {
            SelectBehavior(CurrentBehaviour.Chase);
        }
    }
}
