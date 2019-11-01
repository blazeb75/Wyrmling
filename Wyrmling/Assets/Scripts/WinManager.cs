using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject canvas;

    public static WinManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate death manager instance found", this.gameObject);
            Destroy(this);
        }
    }

    void Start()
    {
        canvas.SetActive(false);
        StartCoroutine(CheckWinState());
    }

    /// <summary>
    /// Every frame, checks whether the player's scale has reached the victory threshold.
    /// If it has, freeze the player and show the victory canvas.
    /// </summary>
    IEnumerator CheckWinState()
    {
        float size = 0;
        while (size < 100)
        {
            yield return null;
            size = PlayerManager.instance.transform.localScale.y / 8f * 100f;
        }
        //The player has grown large enough. Show the canvas until input is recieved.
        canvas.SetActive(true);
        StartCoroutine(WaitForInput());
        //Freeze the player
        Destroy(PlayerManager.instance);
    }

   
    IEnumerator WaitForInput()
    {
        while (true)
        {
            if (InputManager.instance.GetKeyDown("press"))
            {
                SceneManager.LoadScene("Menu");
                StopAllCoroutines();
            }

            yield return null;
        }
    }
}
