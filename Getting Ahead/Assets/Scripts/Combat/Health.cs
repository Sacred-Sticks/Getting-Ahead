using System;
using Kickstarter.Events;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Outgoing Services")]
    [SerializeField] private Service onDeath;

    private float maxHealth;
    public float MaxHealth
    {
        set
        {
            maxHealth = value;
            currentHealth = maxHealth;
        }
    }
    
    private float currentHealth;
    private float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
                onDeath.Trigger(new DeathArgs(gameObject));
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }
    
    public class DeathArgs : EventArgs
    {
        public DeathArgs(GameObject dyingCharacterGameObject)
        {
            DyingCharacterGameObject = dyingCharacterGameObject;
        }
        
        public GameObject DyingCharacterGameObject { get; }
    }
}
