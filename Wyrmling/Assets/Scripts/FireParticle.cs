using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    public float speed = 20;
    public float damage = 10;
    public float duration = 3;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Move
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);

        //Die after existing for duration
        if(Time.time - startTime > duration)
        {
            Destroy(this.gameObject);
        }
    }

    //When another object overlaps with this one, destroy this and send a damage message
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The damage method can be recieved by Health components
        collision.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(GetComponent<Collider2D>());
        StartCoroutine(DestroyAfterDelay());
    }

    //Since many colliders are larger than their object's sprite, 
    //the fire particle will continue to move for a moment after collision before being destroyed
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }
}
