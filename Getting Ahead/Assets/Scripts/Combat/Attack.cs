using System.Collections;
using UnityEngine;
using Kickstarter.Observer;

public abstract class Attack : Observable
{
    [Range(0, 1)]
    [SerializeField] private float inputTolerance;
    [SerializeField] protected Weapon weapon;
    public float AttackRate { get; set; }
    public float AttackRange { get; private set; }
    private bool isAttacking;
    private Coroutine firingRoutine;
    private float rawInput;
    private bool canAttack = true;

    private void Start()
    {
        AttackRange = weapon switch
        {
            MeleeWeapon meleeWeapon => meleeWeapon.AttackRadius,
            RangedWeapon rangedWeapon => rangedWeapon.AttackRange,
            _ => AttackRange,
        };
    }

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
        NotifyObservers(isAttacking);
        switch (isAttacking)
        {
            case true:
                NotifyObservers(isAttacking);
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
