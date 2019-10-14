using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
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
        sizeText = gameOverCanvas.transform.Find("SizeText").GetComponent<Text>();
        gameOverCanvas.SetActive(false);
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        float size = PlayerManager.instance.transform.localScale.y / 20f * 100f;        
        decimal sizeDec = decimal.Round(decimal.Parse(size.ToString()), 2, System.MidpointRounding.AwayFromZero);
        string sizeString = sizeDec.ToString() + "%";
        sizeText.text = sizeText.text.Replace("_SIZE", sizeString);
        StartCoroutine(WaitForDeathInput());

        Destroy(PlayerManager.instance);
    }

    IEnumerator WaitForDeathInput()
    {
        while (true)
        {
            if (InputManager.instance.GetKeyDown("press"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            yield return null;
        }
    }
}
