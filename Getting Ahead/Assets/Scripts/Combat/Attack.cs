using System.Collections;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

public abstract class Attack : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput attackInput;
    [Range(0, 1)]
    [SerializeField] private float inputTolerance;

    [SerializeField] protected Weapon weapon;
    private bool isAttacking;
    private Coroutine firingRoutine;
    private float rawInput;
    private bool canAttack = true;

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
        rawInput = input;
        if (input < inputTolerance && isAttacking)
            ToggleAttack();
        if (input > inputTolerance && !isAttacking)
            ToggleAttack();
    }

    private void ToggleAttack()
    {
        if (!canAttack)
            return;
        isAttacking = !isAttacking;
        switch (isAttacking)
        {
            case true:
                firingRoutine = StartCoroutine(Attacking());
                break;
            case false:
                StartCoroutine(CancelAttack());
                break;
        }
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            var burstRoutine = StartCoroutine(FireBurst());
            yield return new WaitForSeconds(1 / weapon.AttackRate);
            StopCoroutine(burstRoutine);
        }
    }

    private IEnumerator CancelAttack()
    {
        if (firingRoutine == null)
            yield break;
        canAttack = false;
        StopCoroutine(firingRoutine);
        firingRoutine = null;
        yield return new WaitForSeconds(1 / weapon.AttackRate);
        canAttack = true;
        OnAttackInputChange(rawInput);
    }

    protected abstract IEnumerator FireBurst();
}
