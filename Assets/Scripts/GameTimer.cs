using UnityEngine;
using UnityEngine.UI; 

public class GameTimer : MonoBehaviour
{
    public Text timerText; 
    private float startTime;
    private bool timerActive = true;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (timerActive)
        {
            float timeSinceStarted = Time.time - startTime;
            string timeString = FormatTime(timeSinceStarted);
            timerText.text = timeString;
        }
    }

    string FormatTime(float timeToFormat)
    {
        int minutes = (int)(timeToFormat / 60);
        int seconds = (int)(timeToFormat % 60);
        int milliseconds = (int)((timeToFormat * 1000) % 1000);
        return string.Format("Время жизни: " + "{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void PlayerDied()
    {
        StopTimer();
    }
}
