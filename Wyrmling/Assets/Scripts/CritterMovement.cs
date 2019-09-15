using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Collider))]
public class CritterMovement : MonoBehaviour
{
    public float idleTime = 3;
    public float wanderTime = 1.5f;
    [Tooltip("The minimum time the creature will pursue the player. 0 = ignore them.")]
    public float chaseTime = 4;
    public float fleeTime = 4;
    
    public float wanderSpeed = 2;
    public float chaseSpeed = 2.5f;
    public float fleeSpeed = 2.5f;

    GameObject target;

    Food food;    

    // Start is called before the first frame update
    void Start()
    {
        food = GetComponent<Food>();
        StartCoroutine(Ponder());
    }

    //The trigger collider represents the creature's perceptable area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            target = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            target = null;
    }

    IEnumerator Ponder()
    {
        float time = 0;
        while (time < idleTime)
        {
            time += Time.deltaTime;
            if (target != null)
            {
                //Decide whether to flee or chase
                if (food.size > target.GetComponent<Growth>().foodConsumed)
                {
                    StartCoroutine(Chase());
                }
                else
                {
                    StartCoroutine(Flee());
                }
                yield break;
            }
            yield return null;   
        }

        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        //Choose a random direction
        Vector3 dir = Quaternion.Euler(0, 0, Random.Range(0f, 360f)).eulerAngles.normalized;
        //Move in that direction
        float time = 0;
        while (time < wanderTime)
        {
            transform.Translate(dir * wanderSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(Ponder());
    }

    IEnumerator Chase()
    {
        //Chase the target for the set time
        float time = 0;
        while(time < chaseTime)
        {
            //Get the vector to the target
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            //Move in that direction
            transform.Translate(dir * chaseSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(Wander());
    }

    IEnumerator Flee()
    {
        //Get a vector away from the target
        Vector3 dir = transform.position - target.transform.position;
        dir.Normalize();
        //Scatter by a random amount
        float angle = Random.Range(-30, 30);
        dir.x = dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle);
        dir.y = dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle);
        //Move in that direction     
        float time = 0;
        while(time < fleeTime)
        {
            transform.Translate(dir * fleeSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(Ponder());
    }

    
}
