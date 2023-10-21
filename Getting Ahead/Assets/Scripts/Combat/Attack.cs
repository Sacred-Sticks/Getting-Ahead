using System.Collections;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float inputTolerance;

    [SerializeField] protected Weapon weapon;
    private bool isAttacking;
    private Coroutine firingRoutine;
    private float rawInput;
    private bool canAttack = true;

    public void SetAttackingInput(float input)
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
        SetAttackingInput(rawInput);
    }

    protected abstract IEnumerator FireBurst();
}