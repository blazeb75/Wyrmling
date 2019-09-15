using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public enum MouthState {Idle, Biting, Firebreathing};

    //The amount of food that can be eaten in one bite
    public float biteSize = 5;
    public MouthState state = MouthState.Idle;

    private float baseBiteSize;

    private void Start()
    {
        baseBiteSize = biteSize;
        PlayerManager.instance.OnPlayerGrown.AddListener(UpdateBiteSize);
    }

    private void OnTriggerStay(Collider other)
    {
        //Temporary control:
        if(Input.GetMouseButtonDown(1))
        if (state == MouthState.Biting)
        {
            if (other.gameObject.TryGetComponent(out Food food))
            {
                Eat(food);
            }
        }
    }

    void Eat(Food food)
    {
        //If the player is big enough to eat the good whole, do so
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
    }
}
