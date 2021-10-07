using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VisualTimer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;
  
    public VisualTimer(float timeLength)
    {
        this.timeRemaining = timeLength;
    }

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        //list the minutes + seconds 
        //timeToDisplay += 1;
        //float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        //float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //just list the raw seconds
        float seconds = Mathf.CeilToInt(timeToDisplay);    
        timeText.text = string.Format("{1:00}", seconds);
    }

}
