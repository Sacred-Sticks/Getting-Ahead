using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

public abstract class Attack : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput attackInput;
    [Range(0, 1)]
    [SerializeField] private float inputTolerance;

    protected bool isAttacking;

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
        if (input < inputTolerance && isAttacking)
            EndAttack();
        if (input > inputTolerance && !isAttacking)
            StartAttack();
    }

    protected abstract void StartAttack();
    protected abstract void EndAttack();
}
