using System;
using UnityEngine;

public class DailyMeme : MonoBehaviour
{
    [SerializeField] private GameObject dailyMemeUI;
    
    void Start()
    {
        string lastTimeStr = PlayerPrefs.GetString("LastTime");
        DateTime lastTime;

        if (!string.IsNullOrEmpty(lastTimeStr)) lastTime = DateTime.Parse(lastTimeStr);
        else lastTime = DateTime.MinValue;

        if (DateTime.Today > lastTime)
        {
            dailyMemeUI.SetActive(true);
        }
    }

    public void Close()
    {
        PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
    }
}
