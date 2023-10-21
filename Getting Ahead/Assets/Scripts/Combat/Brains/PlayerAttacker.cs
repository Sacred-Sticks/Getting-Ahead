using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Attack))]
public class PlayerAttacker : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput attackInput;

    private Attack attack;

    private void Awake()
    {
        attack = GetComponent<Attack>();
    }
    
    public void SubscribeToInputs(Player player)
    {
        attackInput.SubscribeToInputAction(OnAttackInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        attackInput.UnsubscribeToInputAction(OnAttackInputChange, player.PlayerID);
    }

    private void OnAttackInputChange(float input)
    {
        attack.SetAttackingInput(input);
    }
}
