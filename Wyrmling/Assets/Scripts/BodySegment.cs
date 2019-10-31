using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    //The angle in degrees that this segment's rotation is allowed to vary from its parent
    [SerializeField] float allowance = 30f;

    //The object's rotation is stored at the end of every frame and restored on the next frame.
    //This is done to nullify Unity's usual parent-child rotation inheritance, 
    //since this script is meant to effectively override it.
    Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }
    
    //LateUpdate is used here so that this occurs after the head and torso have performed their rotations for the frame
    void LateUpdate()
    {
        transform.rotation = rotation;
        SemiChildRotation();
        CapRotation();
        rotation = transform.rotation;
    }

    //Rotates towards the parent's rotation in a delayed, decelerating fashion
    void SemiChildRotation()
    {
        
        float distanceToNeutral = Vector3.SignedAngle(transform.up, transform.parent.up, transform.forward);
        transform.Rotate(0, 0, distanceToNeutral * Time.deltaTime * 5f);
        
    }
    
    //Cap the segment's relative rotation to avoid 'kinks' and prevent unnatural-looking configurations
    void CapRotation()
    {
        //Store the rotation in a temporary variable
        Vector3 cappedRotation = transform.localRotation.eulerAngles;
        //If the rotation is beyond an acceptable value, cap it

        if (cappedRotation.z > allowance && cappedRotation.z < 360 - allowance)
        {
            if (cappedRotation.z > 180)
                cappedRotation.z = 360 - allowance;
            else cappedRotation.z = allowance;
        }

        //Write the temporary variable to the actual rotation
        transform.localRotation = Quaternion.Euler(cappedRotation);
    }

}
