using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using Kickstarter.Events;
using Kickstarter.Observer;


public class AnimationStateController2Dim : MonoBehaviour, IObserver<Vector3>
{
    [SerializeField] private Movement _movementSubject;
    [SerializeField] private Vector2Input movementInput;
    private Animator animator;

    private Vector2 rawInput;
    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;
    void Start()
    {
        animator = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("VelocityX", velocityX);
        animator.SetFloat("VelocityZ", velocityZ);
    }

    public void OnNotify(Vector3 velocity)
    {
        velocity = transform.TransformDirection(velocity);
        velocityX = velocity.x;
        velocityZ = velocity.z;
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
