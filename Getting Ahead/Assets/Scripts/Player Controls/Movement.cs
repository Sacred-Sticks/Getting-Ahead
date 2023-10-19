using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour, IInputReceiver
{
    [SerializeField]
    private Vector2Input movementInput;
    
    public float MoveSpeed { private get; set; }
    
    private Vector2 rawInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var movementDirection = new Vector3(rawInput.x, 0f, rawInput.y);
        rb.velocity = movementDirection * MoveSpeed;
    }

    public void ReceiveInput(Vector2 input)
    {
        rawInput = input;
        Debug.Log($"Raw Input set to {rawInput}");
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
