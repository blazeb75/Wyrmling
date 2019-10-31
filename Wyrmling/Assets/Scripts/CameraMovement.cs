using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Zooms out the camera as the player grows
/// </summary>
public class CameraMovement : MonoBehaviour
{
    Transform player;
    float previousZ;
    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.transform;
        previousZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Follow the player
    void Move()
    {
        Vector3 newPos = player.position + player.transform.up * 2f;
        newPos.z = transform.position.z;
        
        newPos.z = previousZ * (PlayerManager.instance.transform.localScale.x / PlayerManager.instance.growth.startScale);

        transform.position = newPos;
        
    }

    /// <summary>
    /// Zooms the camera out over one second depending on the player's size.
    /// </summary>
    public IEnumerator ScaleZ()
    {
        float scale = PlayerManager.instance.transform.localScale.x;
        float oldZ = transform.position.z;
        float newZ = previousZ - (scale - 1);
        float time = 0;
        //Set previous Z, so that new instances of this coroutine use the correct base z value
        previousZ = newZ;
        Vector3 newPos;

        //Interpolate the Z position over one second
        while (time < 1)
        {
            newPos = transform.position;
            newPos.z = Mathf.SmoothStep(oldZ, newZ, time += Time.deltaTime);
            transform.position = newPos;
            yield return null;
        } 
    }
    
}
