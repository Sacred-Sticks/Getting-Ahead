using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.Events;
using Kickstarter.Observer;


public class AnimationStateController2Dim : MonoBehaviour, IObserver<Vector3>, IObserver<bool>
{
    [SerializeField] private Movement _movementSubject;
    [SerializeField] private Vector2Input movementInput;
    private Animator animator;

    private int velocityXHash = Animator.StringToHash("VelocityX");
    private int velocityZHash = Animator.StringToHash("VelocityZ");
    private int isAttackingHash = Animator.StringToHash("isAttacking");

    private Vector2 rawInput;
    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;
    void Awake()
    {
        animator = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Animator>();
        _movementSubject = GetComponent<Movement>();
    }

    public void OnNotify(Vector3 velocity)
    {
        velocity = transform.InverseTransformDirection(velocity);
        velocityX = velocity.x;
        velocityZ = velocity.z;
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);
    }

    public void OnNotify(bool isAttacking)
    {
        animator.SetBool(isAttackingHash, isAttacking);
    }

    private void OnEnable()
    {
        _movementSubject.AddObserver(this);
    }
    private void OnDisable()
    {
        _movementSubject.RemoveObserver(this);
    }
    
}
