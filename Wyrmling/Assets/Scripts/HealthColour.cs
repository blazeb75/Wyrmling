using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Renderer))]
public class HealthColour : MonoBehaviour
{
    public Color damageColor = Color.red;
    public Color deathColor = Color.black;

    private Color startColour;
    private Health health;
    private Renderer render;

    private void Awake()
    {
        health = GetComponent<Health>();
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        startColour = render.material.color;
        health.OnDamaged.AddListener(SetColour);
        health.OnDeath.AddListener(SetDeathColour);
    }

    void SetColour()
    {
        render.material.color = Color.Lerp(damageColor, startColour, health.HitPoints / health.maxHitPoints);
    }

    void SetDeathColour()
    {
        render.material.color = deathColor;
        Destroy(this);
    }
}
