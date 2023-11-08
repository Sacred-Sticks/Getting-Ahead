using UnityEngine;

[RequireComponent(typeof(Attack))]
public class EnemyAttacker : MonoBehaviour, IObserver<EnemyBrain.TriggerAttack>, IObserver<EnemyBrain.TriggerDeath>
{
    private Attack attack;

    #region Unity Events
    private void Awake()
    {
        attack = GetComponent<Attack>();
    }

    private void OnEnable()
    {
        var brain = GetComponent<EnemyBrain>();
        brain.AddObserver<EnemyBrain.TriggerAttack>(this);
        brain.AddObserver<EnemyBrain.TriggerDeath>(this);
    }

    private void OnDisable()
    {
        var brain = GetComponent<EnemyBrain>();
        brain.RemoveObserver<EnemyBrain.TriggerAttack>(this);
        brain.RemoveObserver<EnemyBrain.TriggerDeath>(this);
    }
    #endregion
    
    public void OnNotify(EnemyBrain.TriggerAttack argument)
    {
        attack.SetAttackingInput(argument.AttackActive ? 1 : 0);
    }
    
    public void OnNotify(EnemyBrain.TriggerDeath argument)
    {
        Destroy(this);
    }
}