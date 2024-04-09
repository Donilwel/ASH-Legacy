using UnityEngine;
using UnityEngine.UI; 

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public Text bestTimeText;

    private float startTime;
    private bool timerActive = true;
    private float bestTime;

    private const string BestTimePrefKey = "BestTime";

    void Start()
    {
        startTime = Time.time;
        LoadBestTime();
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
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    void LoadBestTime()
    {
        bestTime = PlayerPrefs.GetFloat(BestTimePrefKey, float.MaxValue);
        bestTimeText.text = bestTime != float.MaxValue ? FormatTime(bestTime) : "N/A";
    }


    public void StopTimer()
    {
        timerActive = false;
    }

    public void PlayerDied()
    {
        StopTimer();

        float currentTime = Time.time - startTime;
        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat(BestTimePrefKey, bestTime);
            PlayerPrefs.Save(); 
            bestTimeText.text = FormatTime(bestTime);
        }
    }
}
