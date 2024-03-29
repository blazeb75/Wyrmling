﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public enum MouthState {Idle, Biting, Firebreathing};

    [SerializeField] GameObject fireParticle;

    //The amount of food that can be eaten in one bite
    public float biteSize = 5;
    public float biteDamage = 50;
    public float biteInterval = 1f;
    public MouthState state = MouthState.Idle;

    public List<GameObject> presentTargets;
    private float baseBiteSize;
    private float baseBiteDamage;

    private void Awake()
    {
        //Initialise list
        presentTargets = new List<GameObject>();
    }

    private void Start()
    {
        baseBiteSize = biteSize;
        baseBiteDamage = biteDamage;
        PlayerManager.instance.OnPlayerGrown.AddListener(UpdateBiteSize);

        StartCoroutine(Biting());
        StartCoroutine(BreatheFire());
    }

    private void Update()
    {
        if (PlayerManager.instance.target != null)
        {
            if (presentTargets.Count > 0)
            {
                state = MouthState.Biting;
            }
            else if (PlayerManager.instance.target.layer == LayerMask.NameToLayer("Creature"))
            {
                state = MouthState.Firebreathing;
            }
            else
            {
                state = MouthState.Idle;
            }
        }
        else
        {
            state = MouthState.Idle;
        }
    }

    //Store references to each creature as long as it stays inside the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        presentTargets.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (presentTargets.Contains(collision.gameObject))
            presentTargets.Remove(collision.gameObject);
    }

    public void Bite()
    {        
        //For each creature inside the mouth collider, damage it and create a bite effect
        //If the creature is dead, eat it instead of damaging it
        foreach(GameObject target in presentTargets)
        {
            if (target != null)
            {                
                if (target.TryGetComponent(out Health health))
                {
                    health.Damage(biteDamage);
                }

                else if (target.gameObject.TryGetComponent(out Food food))
                {
                    Eat(food);
                }
                GameObject bite = Instantiate(Resources.Load<GameObject>("Bite"), transform.position + transform.forward * 2f, Quaternion.identity);
                bite.transform.localScale = PlayerManager.instance.transform.localScale;
                
            }
        }
    }

    //Periodically bite if there is anything to bite and the dragon is in that state
    IEnumerator Biting()
    {
        while (true)
        {
            while (state == MouthState.Biting)
            {
                yield return new WaitForSeconds(biteInterval);
                Bite();
            }
            yield return null;
        }
       
    }

    //Periodically emits flame, as long as the dragon is in that state
    IEnumerator BreatheFire()
    {
        while (true)
        {
            while (Input.GetMouseButton(2) 
                || (state == MouthState.Firebreathing
                //Only breathe fire if the target is alive
                && PlayerManager.instance.target != null))
                {
                //Create a flame particle
                GameObject flame = Instantiate(fireParticle, transform.position, transform.rotation);
                //Randomise its trajectory
                flame.transform.Rotate(0, 0, Random.Range(-10f, 10f));
                yield return new WaitForSeconds(0.05f);
            }
            yield return null;
        }
    }

    //Subtracts food's size and increases the dragon's food consumed
    void Eat(Food food)
    {
        PlayerManager.instance.growth.foodConsumed += Mathf.Min(food.foodValue, biteSize);
        if (food.transform.localScale.x > 0.4)
        {
            food.Bite(biteSize);
        }
        //If the food is very small, finish it off
        //(Without this check, the food can become too small to see)
        else
        {
            food.Bite(Mathf.Infinity);
        }        
        
    }

    //Scales the amount of food consumed per bite to a value to the dragon's size
    public void UpdateBiteSize()
    {
        float scale = PlayerManager.instance.transform.localScale.x;
        biteSize = baseBiteSize * scale;
        biteDamage = baseBiteDamage * scale;
    }
}
