using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public Text finalScoreText;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (finalScoreText != null)
        {
            finalScoreText.text = PlayerPrefs.GetFloat("Final Score").ToString();
        }
        AudioListener.pause = false;
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene("Hackathon");
        
    }

    public void QuitGame ()
    {

        Debug.Log("Quit");
        Application.Quit();
    }

    public void Menu ()
    {
        SceneManager.LoadScene("Menu");
    }
}
