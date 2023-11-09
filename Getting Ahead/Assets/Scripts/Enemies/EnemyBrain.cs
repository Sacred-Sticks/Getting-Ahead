using System.Collections;
using Kickstarter.Observer;
using Kickstarter.StateControllers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class EnemyBrain : Observable, IObserver<Health.DamageTaken>
{
    [SerializeField] private float attackingRange;
    [SerializeField] private float angularSpeed;

    private enum EnemyStatus
    {
        Idle,
        Chasing,
        Attacking,
        Dead,
    }

    public Transform Target { get; set; }
    public float AngularSpeed => angularSpeed;
    private NavMeshAgent agent;
    private Health health;
    
    private StateMachine<EnemyStatus> stateMachine;
    private Coroutine movementRoutine;
    private Coroutine attackingRoutine;
    private PlayerAttacker playerAttacker;

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
        TryGetComponent(out NavMeshAgent navAgent);
        agent = navAgent;
        if (agent == null)
            agent = gameObject.AddComponent<NavMeshAgent>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void Start()
    {
        playerAttacker.enabled = false;
        agent.stoppingDistance = attackingRange;
        agent.angularSpeed = angularSpeed;
        
        stateMachine = new StateMachine<EnemyStatus>.Builder()
            .WithInitialState(EnemyStatus.Idle)
            .WithTransition(EnemyStatus.Idle, EnemyStatus.Chasing)
            .WithTransition(EnemyStatus.Idle, EnemyStatus.Dead)
            .WithTransition(EnemyStatus.Chasing, EnemyStatus.Attacking)
            .WithTransition(EnemyStatus.Chasing, EnemyStatus.Dead)
            .WithTransition(EnemyStatus.Chasing, EnemyStatus.Idle)
            .WithTransition(EnemyStatus.Attacking, EnemyStatus.Chasing)
            .WithTransition(EnemyStatus.Attacking, EnemyStatus.Dead)
            .WithTransition(EnemyStatus.Attacking, EnemyStatus.Idle)
            .WithStateListener(EnemyStatus.Chasing, transitionType.Start, StartChasing)
            .WithStateListener(EnemyStatus.Chasing, transitionType.End, StopChasing)
            .WithStateListener(EnemyStatus.Attacking, transitionType.Start, StartAttacking)
            .WithStateListener(EnemyStatus.Attacking, transitionType.End, StopAttacking)
            .WithStateListener(EnemyStatus.Dead, transitionType.Start, Die)
            .Build();
    }

    private void Update()
    {
        if (stateMachine.CurrentState != EnemyStatus.Attacking)
            return;
        if (!Target)
        {
            stateMachine.CurrentState = EnemyStatus.Idle;
            return;
        }
        float sqrDistance = Vector3.SqrMagnitude(transform.position - Target.position);
        if (sqrDistance > attackingRange * attackingRange)
        {
            stateMachine.CurrentState = EnemyStatus.Chasing;
            return;
        }
        NotifyObservers(new TriggerLookAtTarget());
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
        NotifyObservers(new TriggerAttack(true));
    }

    private void StopAttacking()
    {
        NotifyObservers(new TriggerAttack(false));
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
            if (!Target)
            {
                stateMachine.CurrentState = EnemyStatus.Idle;
                yield break;
            }
            agent.SetDestination(Target.position);
            if (Vector3.SqrMagnitude(transform.position - Target.position) < attackingRange * attackingRange)
                stateMachine.CurrentState = EnemyStatus.Attacking;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnNotify(Health.DamageTaken argument)
    {
        Target = argument.Attacker.transform;
        if (stateMachine.CurrentState == EnemyStatus.Idle)
            stateMachine.CurrentState = EnemyStatus.Chasing;

        if (!(argument.Health <= 0))
            return;
        GetComponent<PlayerAttacker>().enabled = true;
        NotifyObservers(new TriggerDeath());
        stateMachine.CurrentState = EnemyStatus.Dead;
    }

    #region Event Types
    public struct TriggerAttack
    {
        public TriggerAttack(bool attackActive)
        {
            AttackActive = attackActive;
        }
        
        public bool AttackActive { get; }
    }

    public struct TriggerDeath
    {
        
    }

    public struct TriggerLookAtTarget
    {
        
    }
    #endregion
}
