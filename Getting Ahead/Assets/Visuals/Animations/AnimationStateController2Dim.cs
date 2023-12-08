using UnityEngine;

public class AnimationStateController2Dim : MonoBehaviour, IObserver<Vector3>, IObserver<bool>
{
    private Movement movementSubject;
    private Attack attackSubject;
    private Animator animator;

    private readonly int velocityX = Animator.StringToHash("VelocityX");
    private readonly int velocityZ = Animator.StringToHash("VelocityZ");
    private readonly int isAttacking = Animator.StringToHash("IsAttacking");

    private Vector2 rawInput;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnNotify(Vector3 velocity)
    {
        velocity = velocity.normalized;
        velocity = transform.InverseTransformDirection(velocity);
        animator.SetFloat(velocityX, velocity.x);
        animator.SetFloat(velocityZ, velocity.z);
    }

    public void OnNotify(bool argument)
    {
        if (!argument)
        {
            animator.SetLayerWeight(1, 0);
            return;
        }
        animator.SetLayerWeight(1, 1);
        animator.SetTrigger(isAttacking);
    }

    private void OnEnable()
    {
        movementSubject = GetComponent<Movement>();
        attackSubject = GetComponent<Attack>();
        movementSubject.AddObserver(this);
        attackSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        movementSubject.RemoveObserver(this);
        attackSubject.RemoveObserver(this);
    }

}
