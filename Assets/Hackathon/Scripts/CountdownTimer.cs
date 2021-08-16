using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0;
    float startingTime = 90;
    public bool start;
    

    [SerializeField] Text countdownText;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        if(start == true)
        {
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                PlayerPrefs.SetFloat("Final Score", References.levelManager.pointSystem);
                currentTime = 0;
                SceneManager.LoadScene(2);
            }
        }
        
    }
}
