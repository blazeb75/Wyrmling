using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    [SerializeField] float allowance = 30f;
    Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SemiChildRotation();
        CapRotation();
    }

    void SemiChildRotation()
    {
        transform.rotation = rotation;
        float distanceToNeutral = Vector3.SignedAngle(transform.up, transform.parent.up, transform.forward);
        transform.Rotate(0, 0, distanceToNeutral * Time.deltaTime * 5f);
        rotation = transform.rotation;
    }

    //Cap the rotation so the neck doesn't snap
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
        rotation = transform.rotation;
    }
}
