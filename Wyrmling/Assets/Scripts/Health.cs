using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Health contains data and methods associated with hit points and damage for any entity
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The number that hit points cannot exceed")]
    public float maxHitPoints = 100;
    [Tooltip("Health gained per second passively")]
    public float regeneration = 5f;
    [Tooltip("Invoked whenever damage is recieved")]    
    public UnityEvent OnDamaged;
    [Tooltip("Invoked when the entity dies")]
    public UnityEvent OnDeath;
    [Header("Runtime variables")]
    [Tooltip("Exposed to the inspector for testing purposes. Edit in runtime only.")]
    [SerializeField] float hitPoints;

    /// <summary>
    /// Property exposing the creature's current health.
    /// To add or subtract health, use Heal or Damage instead.
    /// </summary>
    public float HitPoints { get => hitPoints; }

    private void Start()
    {
        hitPoints = maxHitPoints;
    }
    private void Update()
    {
        Heal(regeneration * Time.deltaTime);
    }

    /// <summary>
    /// Remove health, potentially inflicting death
    /// </summary>
    /// <param name="amount">The number of hit points to remove</param>
    public void Damage(float amount)
    {
        if (amount > 0)
        {
            hitPoints -= amount;
            OnDamaged.Invoke();
            CheckHP();
        }
        
    }
    /// <summary>
    /// Add health up to MaxHitpoints
    /// </summary>
    /// <param name="amount">The number of hit points to restore</param>
    public void Heal(float amount)
    {
        if (amount > 0)
        {
            hitPoints += amount;
        }
        CheckHP();
    }

    /// <summary>
    /// Inflicts death is hitPoints is less than 0.
    /// Enforces maxHitPoints as a cap for hitPoints
    /// </summary>
    private void CheckHP()
    {
        if (hitPoints <= 0)
            Die();
        else if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;
    }

    /// <summary>
    /// Destroys this component, resulting in a game over if it is the player.
    /// </summary>
    private void Die()
    {
        OnDeath.Invoke();
        if (CompareTag("Player"))
        {            
            DeathManager.instance.GameOver();
        }
        Destroy(this);
        
    }
}
