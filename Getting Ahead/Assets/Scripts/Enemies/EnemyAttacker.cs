using UnityEngine;

public class EnemyAttacker : MonoBehaviour, IObserver<EnemyBrain.TriggerDeath>
{
    public void OnNotify(EnemyBrain.TriggerDeath argument)
    {
        Destroy(this);
    }
}