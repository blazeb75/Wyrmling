using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse
{
    ///<Summary>Gets the worldspace position of the mouse or touch, depending on platform.</Summary>
    public static Vector3 Position
    {
        get
        {
            #if UNITY_IOS
            Vector3 pos = Input.GetTouch(0).position;         
            #else 
            Vector3 pos = Input.mousePosition;
            #endif
            pos.z = Camera.main.transform.position.z * -1f;
            return Camera.main.ScreenToWorldPoint(pos);
        }
    }
}
