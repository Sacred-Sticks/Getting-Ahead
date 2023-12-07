using System;
using System.Collections;
using Kickstarter.Events;
using Kickstarter.Observer;
using UnityEngine;

public class Health : Observable
{
    [Header("Outgoing Services")]
    [SerializeField] private Service onDeath;
    public float IFrames { private get; set; }

    private bool vulnerable = true;

    private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            currentHealth = maxHealth;
        }
    }
    
    private float currentHealth;
    private float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            StartCoroutine(Invunerability());
            if (currentHealth <= 0)
                onDeath.Trigger(new DeathArgs(gameObject));
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (!vulnerable)
            return;
        CurrentHealth -= damage;
        NotifyObservers(new DamageTaken(MaxHealth, CurrentHealth, attacker, gameObject));
    }

    private IEnumerator Invunerability()
    {
        vulnerable = false;
        yield return new WaitForSeconds(IFrames);
        vulnerable = true;
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
        public DamageTaken(float maxHealth, float health, GameObject attacker, GameObject sender)
        {
            MaxHealth = maxHealth;
            Health = health;
            Attacker = attacker;
            Sender = sender;
        }
        public float MaxHealth { get;}
        public float Health { get; }
        public GameObject Attacker { get; }
        public GameObject Sender { get; }
    }
}
