using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HealthColour : MonoBehaviour
{
    [Tooltip("The color that the object will turn when it is 99.99% damaged")]
    public Color damageColor = Color.red;
    [Tooltip("The color that the object will turn when it dies, overwriting damageColor")]
    public Color deathColor = Color.black;

    private Color startColour;
    private Health health;
    private Renderer render;

    private void Awake()
    {
        health = transform.GetComponentInParent<Health>();        
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        //Remember the object's start color
        startColour = render.material.color;
        //Update health color when damage is taken
        health.OnDamaged.AddListener(SetColour);
        //Set death color on death
        health.OnDeath.AddListener(SetDeathColour);

        StartCoroutine(PeriodicallyUpdateColor());
    }

    /// <summary>
    /// Set's the renderer's material color to one between its regular color and damage color based on current hit points
    /// </summary>
    void SetColour()
    {
        render.material.color = Color.Lerp(damageColor, startColour, health.HitPoints / health.maxHitPoints);
    }

    /// <summary>
    /// Sets the renderer's material color to the death color.
    /// </summary>
    void SetDeathColour()
    {
        render.material.color = deathColor;
        Destroy(this);
    }

    IEnumerator PeriodicallyUpdateColor()
    {
        while (true)
        {            
            yield return new WaitForSeconds(1.5f);
            SetColour();
        }
    }
}
