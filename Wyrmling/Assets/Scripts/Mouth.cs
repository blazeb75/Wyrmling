using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public enum MouthState {Idle, Biting, Firebreathing};

    //The amount of food that can be eaten in one bite
    public float biteSize = 5;
    public float biteDamage = 50;
    public MouthState state = MouthState.Idle;

    private float baseBiteSize;
    private float baseBiteDamage;

    private void Start()
    {
        baseBiteSize = biteSize;
        baseBiteDamage = biteDamage;
        PlayerManager.instance.OnPlayerGrown.AddListener(UpdateBiteSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Temporary control:
        if (Input.GetMouseButtonDown(1))
            if (state == MouthState.Biting)
            {
                if (other.TryGetComponent(out Health health))
                {
                    health.Damage(biteDamage);
                }

                else if (other.gameObject.TryGetComponent(out Food food))
                {
                    Eat(food);
                }
            }
    }

    void Eat(Food food)
    {
        //If the player is big enough to eat the food whole, do so
        if (PlayerManager.instance.growth.foodConsumed > food.size)
        {
            PlayerManager.instance.growth.foodConsumed += food.foodValue;
            food.Consume();
        }
        else
        {
            PlayerManager.instance.growth.foodConsumed += Mathf.Min(food.foodValue, biteSize);
            food.Bite(biteSize);
        }
    }

    public void UpdateBiteSize()
    {
        float scale = PlayerManager.instance.transform.localScale.x;
        biteSize = baseBiteSize * scale;
        biteDamage = baseBiteDamage * scale;
    }
}
