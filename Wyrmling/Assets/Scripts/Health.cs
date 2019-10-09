using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHitPoints = 100;
    public UnityEvent OnDamaged;
    public UnityEvent OnDeath;

    private float hitPoints;

    public float HitPoints { get => hitPoints; }

    private void Start()
    {
        hitPoints = maxHitPoints;
    }

    public void Damage(float amount)
    {
        if (amount > 0)
        {
            hitPoints -= amount;
            OnDamaged.Invoke();
            CheckHP();
        }
        
    }

    public void Heal(float amount)
    {
        if (amount > 0)
        {
            hitPoints += amount;
        }
        CheckHP();
    }

    private void CheckHP()
    {
        if (hitPoints <= 0)
            Die();
        else if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;
    }
    private void Die()
    {
        OnDeath.Invoke();
        Destroy(this);
        
    }
}
