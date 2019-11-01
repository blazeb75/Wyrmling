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
    public void GameOver()
    {
        //Show the canvas
        canvas.SetActive(true);
        //Find the text object that shows the player's size so that the correct value can be written to it
        sizeText = canvas.transform.Find("SizeText").GetComponent<Text>();
        //A great wyrm has a scale of 8. Calculate the percentage of this the player has achieved.
        float size = PlayerManager.instance.transform.localScale.y / 8f * 100f;        
        //Round to two decimal places
        decimal sizeDec = decimal.Round(decimal.Parse(size.ToString()), 2, System.MidpointRounding.AwayFromZero);
        //Convert to a string and insert the string into the text object
        string sizeString = sizeDec.ToString() + "%";
        sizeText.text = sizeText.text.Replace("_SIZE", sizeString);
        //Wait for the player's input before leaving this screen
        StartCoroutine(WaitForInput());
        //Make the player actually 'die' (stop moving or reacting to inputs)
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
