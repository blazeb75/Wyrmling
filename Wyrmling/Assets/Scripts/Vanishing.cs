using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanishing : MonoBehaviour
{
    public enum Mode { Shrink }
    public Mode mode;
    public float duration = 3;

    public float age = 0;

    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        age += Time.deltaTime;
        if (age > duration)
        {
            Destroy(this.gameObject);
        }
        else if (mode == Mode.Shrink)
        {
            transform.localScale = startScale * (duration - age) / duration;
        }
    }
}
