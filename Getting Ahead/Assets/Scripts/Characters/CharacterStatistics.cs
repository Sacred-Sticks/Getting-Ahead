using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class CharacterStatistics : MonoBehaviour
{
    [SerializeField] private BodyStatistics bodyStatistics;

    private Movement movement;
    private Attack attack;
    private Health health;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        attack = GetComponent<Attack>();
    }

    public void ApplyValues(HeadStatistics headStats)
    {
        movement.MoveSpeed = bodyStatistics.MoveSpeed * headStats.MoveSpeedMultiplier;
        health.MaxHealth = bodyStatistics.MaxHealth * headStats.MaxHealthMultiplier;
        attack.AttackRate = bodyStatistics.AttackRate * headStats.AttackRateMultiplier;
    }
}
