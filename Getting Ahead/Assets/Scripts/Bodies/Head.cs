using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Head : MonoBehaviour
{
    [SerializeField] private CharacterStatistics characterStatistics;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Start()
    {
        movement.MoveSpeed = characterStatistics.MoveSpeed;
    }
}
