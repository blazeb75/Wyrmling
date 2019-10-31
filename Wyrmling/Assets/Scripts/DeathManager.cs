using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject canvas;
    Text sizeText;

    public static DeathManager instance;

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
        canvas.SetActive(true);
        sizeText = canvas.transform.Find("SizeText").GetComponent<Text>();
        canvas.SetActive(false);
    }

    public void GameOver()
    {
        canvas.SetActive(true);
        float size = PlayerManager.instance.transform.localScale.y / 20f * 100f;        
        decimal sizeDec = decimal.Round(decimal.Parse(size.ToString()), 2, System.MidpointRounding.AwayFromZero);
        string sizeString = sizeDec.ToString() + "%";
        sizeText.text = sizeText.text.Replace("_SIZE", sizeString);
        StartCoroutine(WaitForInput());

        Destroy(PlayerManager.instance);
    }

    IEnumerator WaitForInput()
    {
        while (true)
        {
            if (InputManager.instance.GetKeyDown("press"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                StopAllCoroutines();
            }

            yield return null;
        }
    }
}
