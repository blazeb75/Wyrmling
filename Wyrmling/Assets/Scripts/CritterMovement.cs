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
    public float biteDamage = 10f;

    GameObject target;

    Food foodComponent;

    Collider2D col;

    //The list used to store raycast hits from collision detection
    //Having this as a field rather than a local variable has some performance benefit (or so I've heard)
    List<RaycastHit2D> hits;

    //The filter used for collision detection
    ContactFilter2D collisionFilter;

    private void Awake()
    {
        foodComponent = GetComponent<Food>();

        col = GetComponent<Collider2D>();

        collisionFilter.NoFilter();
        collisionFilter.useLayerMask = true;
        collisionFilter.layerMask = LayerMask.GetMask("Environment", "Player", "Creature");

        //Initialise list
        hits = new List<RaycastHit2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnDeath.AddListener(DestroyThis);
        StartCoroutine(Ponder());
        StartCoroutine(Biting());
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
        //When this component is removed, the creature becomes food
        gameObject.layer = LayerMask.NameToLayer("Food");
    }

    //This method allows destroy to be a listener to a unity event by having no parameter
    void DestroyThis()
    {
        Destroy(this);
    }

    bool DecideBehaviour()
    {
        if (target != null)
        {
            //Decide whether to flee or chase
            if (transform.localScale.x * aggressionMultiplier > target.transform.localScale.x)
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

    //The creature waits in place.
    //It will be interupted if the player is nearby
    //Otherwise, it will Wander after idleTime has passed
    IEnumerator Ponder()
    {
        float time = 0;
        while (time < idleTime)
        {
            //Check if the player is nearby and stop this behaviour if they are
            if (DecideBehaviour())
            {
                yield break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Wander());
    }

    //The creature moves in a random direction
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

    //The creature chases the player
    IEnumerator Chase()
    {
        //Chase the target for the set time
        float time = 0;
        GameObject rememberedTarget = target;
        while (time < chaseTime)
        {
            //Get the vector to the target
            Vector3 dir = rememberedTarget.transform.position - transform.position;
            dir.Normalize();
            //Move in that direction
            transform.up = dir;
            //Check collision
            if (col.Cast(dir, collisionFilter, hits, chaseSpeed * Time.deltaTime) == 0)
            {
                transform.Translate(dir * chaseSpeed * Time.deltaTime, Space.World);
            }
            time += Time.deltaTime;
            yield return null;
        }
        if (!DecideBehaviour())
        {
            StartCoroutine(Ponder());
        }
    }

    //The creature flees from the player
    IEnumerator Flee()
    {
        //Get a vector away from the target
        Vector3 dir = transform.position - target.transform.position;
        dir.Normalize();
        //Scatter by a random amount
        float angle = Random.Range(-30, 30);
        dir = Quaternion.AngleAxis(angle, Vector3.back) * dir;
        //Move in that direction     
        float time = 0;
        while (time < fleeTime)
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

    //This routine periodically bites the player if they are just in front of the creature's collider
    IEnumerator Biting()
    {
        while (true)
        {
            if (target != null) {
                while (Physics2D.Raycast(transform.position, transform.up,  col.bounds.extents.y * (0.25f + transform.localScale.y), LayerMask.GetMask("Player")))
                {
                    Bite();
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return null;
        }

    }

    //Damage the player and create a Bite effect
    public void Bite()
    {
        if (target != null)
        {
            if (target.TryGetComponent(out Health health))
            {
                health.Damage(biteDamage);
            }
            GameObject bite = Instantiate(Resources.Load<GameObject>("Bite"), transform.position + transform.forward * 2f * transform.localScale.y, Quaternion.identity);
            if (PlayerManager.instance != null)
            {
                bite.transform.localScale = PlayerManager.instance.transform.localScale;
            }
        }

    }

}
