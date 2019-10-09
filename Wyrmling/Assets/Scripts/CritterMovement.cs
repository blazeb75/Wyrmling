using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Food))]
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
    public float aggressionMultiplier = 1f;

    GameObject target;

    Food food;

    Collider2D col;
    List<RaycastHit2D> hits;
    ContactFilter2D collisionFilter;

    private void Awake()
    {
        food = GetComponent<Food>();

        col = GetComponent<Collider2D>();
        collisionFilter.layerMask = LayerMask.GetMask("Environment", "Player");
        //Initialise list
        hits = new List<RaycastHit2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnDeath.AddListener(DestroyThis);
        StartCoroutine(Ponder());
    }

    //The trigger collider represents the creature's perceptable area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            target = other.gameObject;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == target)
            target = null;
    }

    private void OnDestroy()
    {
        gameObject.layer = LayerMask.NameToLayer("Food");
    }

    void DestroyThis()
    {
        Destroy(this);
    }

    bool DecideBehaviour()
    {
        if (target != null)
        {
            //Decide whether to flee or chase
            //if (food.size > target.GetComponent<Growth>().foodConsumed)
            if(transform.localScale.x * aggressionMultiplier > target.transform.localScale.x / 2f)
            {
                StartCoroutine(Chase());
                return true;
            }
            else
            {
                StartCoroutine(Flee());
                return true;
            }
        }
        return false;
    }

    IEnumerator Ponder()
    {
        float time = 0;
        while (time < idleTime)
        {
            if (DecideBehaviour())
            {
                yield break;
            }
            time += Time.deltaTime;
            yield return null;   
        }

        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        //Choose a random direction
        transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
        //Move in that direction
        float time = 0;
        //Wander for the duration or until you hit a wall
        while (time < wanderTime
            && col.Cast(transform.up, collisionFilter, hits, wanderSpeed) == 0)
        {
            transform.Translate(transform.up * wanderSpeed * Time.deltaTime, Space.World);
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Ponder());
    }

    IEnumerator Chase()
    {
        //Chase the target for the set time
        float time = 0;
        GameObject rememberedTarget = target;
        while(time < chaseTime
            && col.Cast(transform.up, collisionFilter, hits, chaseSpeed) == 0)
        {
            //Get the vector to the target
            Vector3 dir = rememberedTarget.transform.position - transform.position;
            dir.Normalize();
            //Move in that direction
            transform.up = dir;
            transform.Translate(dir * chaseSpeed * Time.deltaTime, Space.World);
            time += Time.deltaTime;
            yield return null;
        }
        if (!DecideBehaviour())
        {
            StartCoroutine(Ponder());
        }
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
            transform.up = dir;
            transform.Translate(dir * fleeSpeed * Time.deltaTime, Space.World);
            time += Time.deltaTime;
            yield return null;
        }
        if (!DecideBehaviour())
        {
            StartCoroutine(Ponder());
        }
    }

    
}
