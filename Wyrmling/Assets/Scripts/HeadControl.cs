using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadControl : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        LookAtCursor();
        CapRotation();

        PlayerManager.instance.headForward = transform.up;
    }
    //Look at the cursor position
    void LookAtCursor()
    {
        transform.up = Mouse.Position() - transform.position;
        PlayerManager.instance.targetForward = transform.up;
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
