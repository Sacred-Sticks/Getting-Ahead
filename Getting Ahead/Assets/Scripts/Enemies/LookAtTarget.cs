using UnityEngine;

public class LookAtTarget : MonoBehaviour, IObserver<EnemyBrain.TriggerLookAtTarget>, IObserver<EnemyBrain.TriggerDeath>
{
    private float angularSpeed;
    private EnemyBrain brain;
    
    #region Unity Events
    private void OnEnable()
    {
        brain = GetComponent<EnemyBrain>();
        brain.AddObserver<EnemyBrain.TriggerLookAtTarget>(this);
        brain.AddObserver<EnemyBrain.TriggerDeath>(this);
    }

    private void OnDisable()
    {
        brain.RemoveObserver<EnemyBrain.TriggerLookAtTarget>(this);
        brain.RemoveObserver<EnemyBrain.TriggerDeath>(this);
    }

    private void Start()
    {
        angularSpeed = GetComponent<EnemyBrain>().AngularSpeed;
    }
    #endregion

    public void OnNotify(EnemyBrain.TriggerLookAtTarget argument)
    {
        var targetRotation = Quaternion.LookRotation(brain.Target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, angularSpeed * Time.deltaTime);
    }

    public void OnNotify(EnemyBrain.TriggerDeath argument)
    {
        Destroy(this);
    }
}