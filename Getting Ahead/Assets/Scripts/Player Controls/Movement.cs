using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector2Input movementInput;
    private Vector2 rawDirection;
    private Player player;
    private Rigidbody rb;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementInput.SubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    private void FixedUpdate()
    {
        var movementDirection = new Vector3(rawDirection.x, 0f, rawDirection.y);
        rb.velocity = movementDirection * moveSpeed;
    }

    private void RecieveInput(Vector2 input)
    {
        rawDirection = input;
    }
}
