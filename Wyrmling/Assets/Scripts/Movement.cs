using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Inspector variables")]
    public float rotationSpeed = 120f;
    public float moveSpeed = 5f;
    public float aggressiveStoppingDistance = 1f;
    public float passiveStoppingDistance = 5f;
    public float stoppingRotationDistance = 30f;
    public GameObject nose;

    [Header("Debugging variables, do not edit.")]
    public float currentSpeed;
    public float currentRotationSpeed;

    Collider2D col;
    List<RaycastHit2D> hits;
    ContactFilter2D collisionFilter;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        collisionFilter.NoFilter();
        collisionFilter.useLayerMask = true;
        collisionFilter.layerMask = LayerMask.GetMask("Environment");
        //Initialise list
        hits = new List<RaycastHit2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
        MoveTowardsHeadForward();              
    }

    void RotateTowardsMouse()
    {        
        currentRotationSpeed = rotationSpeed * Time.deltaTime;
        //Reduce speed if the remaining angle is small
        float angleToMouse = Vector3.SignedAngle(transform.up, PlayerManager.instance.TargetPosition - transform.position, transform.forward);
        if (Mathf.Abs(angleToMouse) < stoppingRotationDistance)
        {
            currentRotationSpeed *= angleToMouse / stoppingRotationDistance;
        }
        //Ensure the rotation is negative if appropriate
        else if (angleToMouse < 0) currentRotationSpeed *= -1f;
        //Actually rotate
        transform.Rotate(0,0,currentRotationSpeed);
    }

    void MoveTowardsHeadForward()
    {
        currentSpeed = moveSpeed * Time.deltaTime * transform.localScale.y;
        Vector3 direction = PlayerManager.instance.headForward;
        //If the target is behind the nose, reverse towards it
        if(Vector3.Distance(transform.position, PlayerManager.instance.TargetPosition) < Vector3.Distance(transform.position, nose.transform.position))
        {
            currentSpeed *= -0.5f;
            direction = transform.up;
        }

        //If close to target, slow down to stop
        float noseDistance = Vector3.Distance(nose.transform.position, PlayerManager.instance.TargetPosition);
        if (noseDistance < aggressiveStoppingDistance * transform.localScale.y)
        {
            if (PlayerManager.instance.target != null)
            {
                currentSpeed *= noseDistance / (aggressiveStoppingDistance * 0.8f);
            }
            else
            {
                currentSpeed *= noseDistance / (passiveStoppingDistance * 0.8f);
            }
            //Sleep if very close
            if (noseDistance < 0.05f) return;
        }
        //Check collision       
        if (col.Cast(direction, collisionFilter, hits, currentSpeed) != 0)
        {
            return;
        }

        //Actually move
        transform.Translate(direction * currentSpeed, Space.World);
    }
}
