﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadControl : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    Vector3 rotationLastFrame;

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMouse();
        CapRotation();

        PlayerManager.instance.headForward = transform.up;
    }
    //Look towards the cursor position
    void RotateTowardsMouse()
    {
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        //Adjust speed according to distance
        float distanceToMouse = Vector3.SignedAngle(transform.up, Mouse.Position() - transform.position, transform.forward);        
        //Ensure the rotation is negative if appropriate
        if (distanceToMouse < 0) rotationThisFrame *= -1;
        //Prevent overshooting
        if (Mathf.Abs(distanceToMouse) < Mathf.Abs(rotationThisFrame))
        {
            rotationThisFrame = distanceToMouse;
        } 
        //Actually rotate
        transform.Rotate(0, 0, rotationThisFrame);
    }
    //Cap the rotation so the neck doesn't snap
    void CapRotation()
    {
        //Store the rotation in a temporary variable
        Vector3 cappedRotation = transform.localRotation.eulerAngles;
        //If the rotation is beyond an acceptable value, cap it

        if (cappedRotation.z > 75 && cappedRotation.z < 285)
        {            
            if (cappedRotation.z > 180)
                cappedRotation.z = 285;
            else cappedRotation.z = 75;
        }

        //Write the temporary variable to the actual rotation
        transform.localRotation = Quaternion.Euler(cappedRotation);
    }
}