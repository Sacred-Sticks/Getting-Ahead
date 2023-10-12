using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class CharacterStatistics : MonoBehaviour
{
    [SerializeField] private HeadStatistics headStatistics;

    private Movement movement;
    private Health health;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        movement.MoveSpeed = headStatistics.MoveSpeedMultiplier;
        health.MaxHealth = headStatistics.MaxHealthMultiplier;
    }
}
