using System;
using System.Linq;
using Kickstarter.Categorization;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Rigidbody body;

    public GameObject Source { private get; set; }
    public CategoryType TargetCategory { private get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        body.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collidedObject = collision.gameObject;
        if (collidedObject == Source)
            return;
        collidedObject.TryGetComponent(out ObjectCategories objectCategories);
        if (objectCategories != null)
            DealDamage(objectCategories, collidedObject);
        Destroy(gameObject);
    }

    private void DealDamage(ObjectCategories objectCategories, GameObject collidedObject)
    {
        if (!objectCategories.Categories.Contains(TargetCategory))
            return;
        collidedObject.TryGetComponent(out Health health);
        if (health == null)
            return;
        health.TakeDamage(damage);
    }
}
