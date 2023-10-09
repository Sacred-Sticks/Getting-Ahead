using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour, IInputReceiver<Vector2>
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector2Input movementInput;
    private Vector2 rawInput;
    private Player player;
    private Rigidbody rb;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SubscribeToInputs(player);
    }

    private void OnDisable()
    {
        UnsubscribeToInputs(player);
    }

    private void Start()
    {
        SubscribeToInputs(player);
    }

    private void FixedUpdate()
    {
        var movementDirection = new Vector3(rawInput.x, 0f, rawInput.y);
        rb.velocity = movementDirection * moveSpeed;
    }

    public void ReceiveInput(Vector2 input)
    {
        rawInput = input;
    }

    public void ResetInputs(Player oldPlayer, Player newPlayer)
    {
        UnsubscribeToInputs(oldPlayer);
        SubscribeToInputs(newPlayer);
    }

    public void SubscribeToInputs(Player player)
    {
        movementInput.SubscribeToInputAction(ReceiveInput, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        movementInput.UnsubscribeToInputAction(ReceiveInput, player.PlayerID);
    }
}
