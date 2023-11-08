using UnityEngine;

public class HeadPair : MonoBehaviour, IObserver<EnemyBrain.TriggerDeath>
{
    public GameObject Head { get; set; }
    [SerializeField] private Transform headRoot;
    
    public Transform HeadRoot => headRoot;

    #region Unity Events
    private void OnEnable()
    {
        TryGetComponent(out EnemyBrain brain);
        if (brain == null)
            return;
        brain.AddObserver(this);
    }

    private void OnDisable()
    {
        TryGetComponent(out EnemyBrain brain);
        if (brain == null)
            return;
        brain.RemoveObserver(this);
    }
    #endregion

    public void OnNotify(EnemyBrain.TriggerDeath argument)
    {
        Destroy(Head);
    }
}
