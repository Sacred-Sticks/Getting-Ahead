using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Rotation : MonoBehaviour, IInputReceiver
{
    [SerializeField]
    private Vector2Input rotationInput;
    [SerializeField]
    [Range(0, 1)]
    private float deadzone;
    
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
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

    public void ResetInputs(Player oldPlayer, Player newPlayer)
    {
        UnsubscribeToInputs(oldPlayer);
        SubscribeToInputs(newPlayer);
    }

    public void SubscribeToInputs(Player player)
    {
        rotationInput.SubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        rotationInput.UnsubscribeToInputAction(RecieveInput, player.PlayerID);
    }

    private void RecieveInput(Vector2 input)
    {
        if (input.sqrMagnitude == 0)
            return;
        if (input.sqrMagnitude < deadzone * deadzone) // Squared for performance sake
            return;
        float angleA = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angleA, 0f);
    }
}
