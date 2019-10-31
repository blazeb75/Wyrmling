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

    IEnumerator CheckWinState()
    {
        float size = 0;
        while (size < 100)
        {
            yield return null;
            size = PlayerManager.instance.transform.localScale.y / 20f * 100f;
        }

        canvas.SetActive(true);
        StartCoroutine(WaitForInput());

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
