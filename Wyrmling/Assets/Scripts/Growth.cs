using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{
    [Header("Inspector variables")]
    public float scalePerFoodValue = 0.025f;

    [Header("Runtime variables")]
    public float foodConsumed = 0;
    public float startScale;
    float startHP;

    private void Start()
    {
        startHP = PlayerManager.instance.health.maxHitPoints;
        startScale = transform.localScale.x;
        PlayerManager.instance.OnFoodConsumed.AddListener(StartGrow);
    }

    void StartGrow()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        //Calculate the scale once 'digestion' is finished
        float nextScale = startScale + scalePerFoodValue * foodConsumed;
        float prevscale = transform.localRotation.x;
        while (transform.localScale.x < nextScale)
        {
            //Calculate the scale this frame
            float newScale = transform.localScale.x + (nextScale - prevscale) * (Time.deltaTime / 3f);
            
            //Prevent overshooting
            if (newScale >= nextScale)
            {
                newScale = nextScale;
                PlayerManager.instance.OnPlayerGrown.Invoke();
            }
            transform.localScale = new Vector3(newScale, newScale, newScale);
            PlayerManager.instance.health.maxHitPoints = transform.localScale.x * startHP / startScale;
            yield return null;
        }
    }
}
