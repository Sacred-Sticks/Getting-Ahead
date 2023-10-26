using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.Events;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour, IInputReceiver
{
    [SerializeField]
    private Vector2Input movementInput;

    [SerializeField] private Service onAudioTrigger;
    [SerializeField] private string movementSound;
    
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
        onAudioTrigger.Trigger(new AudioManager.AudioArgs(gameObject, movementSound));
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
