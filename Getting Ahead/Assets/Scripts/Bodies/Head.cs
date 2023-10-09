using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class Head : MonoBehaviour
{
    [SerializeField] private CharacterStatistics characterStatistics;

    private Movement movement;
    private Health health;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        movement.MoveSpeed = characterStatistics.MoveSpeed;
        health.MaxHealth = characterStatistics.MaxHealth;
    }
}
