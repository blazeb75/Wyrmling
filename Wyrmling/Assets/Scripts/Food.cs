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

    public virtual IEnumerator Consume()
    {
        PlayerManager.instance.OnFoodConsumed.Invoke();
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
        yield return null;
        Destroy(gameObject);
    }

    public virtual void Bite(float amount)
    {
        foodValue -= amount;
        if (foodValue <= 0)
            StartCoroutine(Consume());
        else
        {
            Vector3 newScale = transform.localScale;
            newScale *= foodValue / startFoodValue;
            transform.localScale = newScale;
        }
    }
}
