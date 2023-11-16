using System;
using System.Collections;
using Kickstarter.Events;
using Kickstarter.Observer;
using Unity.VisualScripting;
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
        NotifyObservers(new DamageTaken(CurrentHealth, attacker));
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
        public DamageTaken(float health, GameObject attacker)
        {
            Health = health;
            Attacker = attacker;
        }
        
        public float Health { get; }
        public GameObject Attacker { get; }
    }
}
