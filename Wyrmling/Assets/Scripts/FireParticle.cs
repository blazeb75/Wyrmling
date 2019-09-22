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
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);

        //Die after duration
        if(Time.time - startTime > duration)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);

    }
}
