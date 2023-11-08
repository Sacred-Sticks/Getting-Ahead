using System.Collections;
using Kickstarter.StateControllers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IObserver<Health.DamageTaken>
{
    private enum EnemyStatus
    {
        Idle,
        Chasing,
        Attacking,
        Dead,
    }

    private Transform target;
    private NavMeshAgent agent;
    private Health health;
    
    private StateMachine<EnemyStatus> stateMachine;
    private Coroutine movementRoutine;
    private Coroutine attackingRoutine;

    #region Unity Events
    private void OnEnable()
    {
        health = GetComponent<Health>();
        health.AddObserver(this);
    }

    private void OnDisable()
    {
        health.RemoveObserver(this);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<EnemyStatus>.Builder()
            .WithInitialState(EnemyStatus.Idle)
            .WithTransition(EnemyStatus.Idle, EnemyStatus.Chasing)
            .WithTransition(EnemyStatus.Idle, EnemyStatus.Dead)
            .WithTransition(EnemyStatus.Chasing, EnemyStatus.Attacking)
            .WithTransition(EnemyStatus.Chasing, EnemyStatus.Dead)
            .WithTransition(EnemyStatus.Attacking, EnemyStatus.Chasing)
            .WithTransition(EnemyStatus.Attacking, EnemyStatus.Dead)
            .WithStateListener(EnemyStatus.Chasing, transitionType.Start, StartChasing)
            .WithStateListener(EnemyStatus.Chasing, transitionType.End, StopChasing)
            .WithStateListener(EnemyStatus.Attacking, transitionType.Start, StartAttacking)
            .WithStateListener(EnemyStatus.Attacking, transitionType.End, StopAttacking)
            .WithStateListener(EnemyStatus.Dead, transitionType.Start, Die)
            .Build();
    }
    #endregion

    #region State Changes
    private void StartChasing()
    {
        movementRoutine ??= StartCoroutine(ChaseTarget());
    }

    private void StopChasing()
    {
        if (movementRoutine == null)
            return;
        StopCoroutine(movementRoutine);
        movementRoutine = null;
    }

    private void StartAttacking()
    {
        attackingRoutine ??= StartCoroutine(AttackTarget());
    }

    private void StopAttacking()
    {
        if (attackingRoutine == null)
            return;
        StopCoroutine(attackingRoutine);
        attackingRoutine = null;
    }

    private void Die()
    {
        Destroy(agent);
        Destroy(this);
    }
    #endregion

    private IEnumerator ChaseTarget()
    {
        while (true)
        {
            agent.SetDestination(target.position);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator AttackTarget()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void OnNotify(Health.DamageTaken argument)
    {
        Debug.Log("Health Changed");
        if (argument.Health <= 0)
            stateMachine.CurrentState = EnemyStatus.Dead;
    }
}
