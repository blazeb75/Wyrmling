using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Tooltip("The food value the player must accumulate to be able to eat this whole.")]
    public float size = Mathf.Infinity;

    public float foodValue = 10;

    private float startFoodValue;

    private void Start()
    {
        startFoodValue = foodValue;
    }

    public virtual void Consume()
    {
        PlayerManager.instance.OnFoodConsumed.Invoke();
        gameObject.SetActive(false);
        Destroy(gameObject, 0.001f);
    }

    public virtual void Bite(float amount)
    {
        foodValue -= amount;
        if (foodValue <= 0)
            Consume();
        else
        {
            Vector3 newScale = transform.localScale;
            newScale *= foodValue / startFoodValue;
            transform.localScale = newScale;
        }
    }
}
