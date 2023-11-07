using System;
using Kickstarter.Events;
using UnityEngine;

public class Health : Observable
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

    public void TakeDamage(float damage, GameObject attacker)
    {
        CurrentHealth -= damage;
        NotifyObservers(new DamageTaken(CurrentHealth, attacker));
    }
    
    public class DeathArgs : EventArgs
    {
        public DeathArgs(GameObject dyingCharacterGameObject)
        {
            DyingCharacterGameObject = dyingCharacterGameObject;
        }
        
        public GameObject DyingCharacterGameObject { get; }
    }

    public class DamageTaken
    {
        public DamageTaken(float health, GameObject attacker)
        {
            Health = health;
            Attacker = attacker;
        }
        
        public float Health { get; }
        public GameObject Attacker { get; }
    }
}
