using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHitPoints = 100;
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
        }
        GetComponent<Renderer>().material.color = Color.yellow;
        CheckHP();
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
        Destroy(this);
        Destroy(GetComponent<CritterMovement>());
        GetComponent<Renderer>().material.color = Color.red;
    }
}
