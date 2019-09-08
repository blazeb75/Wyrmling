using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse
{
    //TODO mobile support
    public static Vector3 Position()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
