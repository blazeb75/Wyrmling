using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Inspector variables")]
    public float rotationSpeed = 120f;
    public float moveSpeed = 5f;
    public float stoppingDistance = 3f;
    public float stoppingRotationDistance = 30f;
    public GameObject nose;

    [Header("Debugging variables, do not edit.")]
    public float currentSpeed;
    public float currentRotationSpeed;

    Rigidbody body;
    CharacterController controller;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
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
        currentRotationSpeed = rotationSpeed * Time.deltaTime;
        //Reduce speed if the remaining angle is small
        float angleToMouse = Vector3.SignedAngle(transform.up, InputManager.instance.lastMousePosition - nose.transform.position, transform.forward);
        if (Mathf.Abs(angleToMouse) < stoppingRotationDistance)
        {
            currentRotationSpeed *= angleToMouse / stoppingRotationDistance;
            currentRotationSpeed += 1;
            //Sleep if very close TODO make this work
            if (Mathf.Abs(angleToMouse) < 0.01f) return;
        }        
        //Increase rotation speed if distance is small TODO
        //float distance = Vector3.Distance(nose.transform.position, InputManager.instance.lastMousePosition);
        //Ensure the rotation is negative if appropriate
        if (angleToMouse < 0) currentRotationSpeed *= -1;
        //Actually rotate
        transform.Rotate(0,0,currentRotationSpeed);
    }

    void MoveTowardsHeadForward()
    {
        currentSpeed = moveSpeed * Time.deltaTime;
        //If close to target, slow down to stop
        float distance = Vector3.Distance(nose.transform.position, InputManager.instance.lastMousePosition);
        if (distance < stoppingDistance)
        {
            currentSpeed *= distance / stoppingDistance;
            //Sleep if very close
            if (distance < 0.1f) return;
        }
        //Actually move
        transform.Translate(PlayerManager.instance.headForward * currentSpeed,Space.World);
        //body.MovePosition(transform.position + PlayerManager.instance.headForward * currentSpeed);
        //transform.Translate((InputManager.instance.lastMousePosition - nose.transform.position).normalized * currentSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, InputManager.instance.lastMousePosition,currentSpeed);
        //controller.Move(PlayerManager.instance.headForward * currentSpeed);
    }
}
