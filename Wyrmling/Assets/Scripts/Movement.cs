using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Inspector variables")]
    public float rotationSpeed = 120f;
    public float moveSpeed = 5f;
    public float stoppingDistance = 3f;
    public float stoppingRotationDistance = 30f;

    [Header("Debugging variables, do not edit.")]
    public float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
        if (InputManager.instance.GetKey("press"))
        {
            MoveTowardsHeadForward();
        }
    }

    void RotateTowardsMouse()
    {
        
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        //Adjust speed according to distance
        float distanceToMouse = Vector3.SignedAngle(transform.up, InputManager.instance.lastMousePosition - transform.position,transform.forward);
        Debug.Log(distanceToMouse);        
        if (Mathf.Abs(distanceToMouse) < stoppingRotationDistance)
        {
            rotationThisFrame *= distanceToMouse / stoppingRotationDistance;
        }

        //Ensure the rotation is negative if appropriate
        else if (distanceToMouse < 0) rotationThisFrame *= -1;
        //Actually rotate
        transform.Rotate(0,0,rotationThisFrame);
    }
    void MoveTowardsHeadForward()
    {
        currentSpeed = moveSpeed * Time.deltaTime;
        //If close to target, slow down to stop
        float distance = Vector3.Distance(transform.position, InputManager.instance.lastMousePosition);
        if (distance < stoppingDistance)
        {
            currentSpeed *= distance / stoppingDistance;
        }
        //Actually move
        transform.Translate(PlayerManager.instance.headForward * currentSpeed);
    }
}
