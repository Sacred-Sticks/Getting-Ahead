using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputDemo : MonoBehaviour
{
    [SerializeField] private Vector2Input inputs;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        inputs.SubscribeToInputAction(TakeInputs, player.PlayerID);
    }

    private void TakeInputs(Vector2 input)
    {
        Debug.Log(input);
    }
}
