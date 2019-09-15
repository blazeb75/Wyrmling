using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{
    [Header("Inspector variables")]
    public float scalePerFoodValue = 0.025f;

    [Header("Runtime variables")]
    public float foodConsumed = 0;

    private void Start()
    {
        PlayerManager.instance.OnFoodConsumed.AddListener(StartGrow);
    }

    void StartGrow()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        //Calculate the scale once 'digestion' is finished
        float nextScale = 1 + scalePerFoodValue * foodConsumed;
        while (transform.localScale.x < nextScale)
        {
            //Calculate the scale this frame
            float newScale = transform.localScale.x + scalePerFoodValue * Time.deltaTime * 2f;
            
            //Prevent overshooting
            if (newScale >= nextScale)
            {
                newScale = nextScale;
                PlayerManager.instance.OnPlayerGrown.Invoke();
            }
            transform.localScale = new Vector3(newScale, newScale, newScale);

            yield return null;
        }
    }
}
