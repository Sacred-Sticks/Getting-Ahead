using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.Events;
using Kickstarter.Observer;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : Observable, IInputReceiver
{
    [SerializeField]
    private Vector2Input movementInput;

    [SerializeField] private Service onAudioTrigger;
    [SerializeField] private string movementSound;
    [SerializeField] [Range(0, 1)] private float deadzone = 0.1f;
    
    public float MoveSpeed { get; set; }
    
    private Vector2 rawInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var movementDirection = new Vector3(rawInput.x, 0, rawInput.y);
        var velocity = movementDirection * MoveSpeed + Vector3.up * rb.velocity.y;
        if (velocity.sqrMagnitude > deadzone * deadzone)
        {
            rb.velocity = velocity;
            if (movementDirection != Vector3.zero)
            {
                NotifyObservers(PlayerActions.Moving);
            }
        }
        else
        {
            NotifyObservers(PlayerActions.STOP);
            rb.velocity = Vector3.zero;
        }

        NotifyObservers(rb.velocity);
    }

    public void ReceiveInput(Vector2 input)
    {
        rawInput = input;
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
