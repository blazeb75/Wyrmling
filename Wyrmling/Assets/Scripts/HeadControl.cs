using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadControl : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    Vector3 rotationLastFrame;

    /// <summary>
    /// Lock: The head looks at the selected target position, changed by clicking.
    /// Free: The head looks at the cursor at all times, regardless of clicks. 
    /// Free is not guarenteed to function properly with other game mechanics such as fire breathing.
    /// </summary>
    public enum LookMode { Lock, Free}
    public LookMode lookMode = LookMode.Lock;

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
        //Get the position to look at, depending on settings
        Vector3 targetPos = new Vector3();
        if(lookMode == LookMode.Free)
        {
            targetPos = Mouse.Position;
        }
        else if (lookMode == LookMode.Lock)
        {
            targetPos = PlayerManager.instance.TargetPosition;
        }

        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        //Adjust speed according to distance
        float distanceToMouse = Vector3.SignedAngle(transform.up, targetPos - transform.position, transform.forward);        
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
        //Store the rotation in a temporary variable so it can be edited (members of struct properties cannot be written to)
        Vector3 cappedRotation = transform.localRotation.eulerAngles;
        //If the rotation is beyond an acceptable value, cap it

        if (cappedRotation.z > 45 && cappedRotation.z < 360 - 45)
        {            
            if (cappedRotation.z > 180)
                cappedRotation.z = 360 - 45;
            else cappedRotation.z = 45;
        }

        //Write the temporary variable to the actual rotation
        transform.localRotation = Quaternion.Euler(cappedRotation);
    }
}
