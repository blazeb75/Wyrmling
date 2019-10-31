using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    public string sceneName = "MainScene";

    private void Update()
    {
        if (InputManager.instance.GetKeyDown("press"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
