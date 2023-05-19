using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private int seconds = 0;
    private int minute = 0;

    private void Start()
    {
        StartCoroutine(Time());
    }

    private void UpdateTexts()
    {
        string temps = seconds.ToString();
        string tempm = minute.ToString(); 

        if (seconds < 10) temps = "0" + seconds.ToString();
        if (minute < 10) tempm = "0" + minute.ToString();

        _timerText.text = tempm + ":" + temps;
    }

    private IEnumerator Time()
    {
        while (true)
        {
            seconds++;
            if (seconds >= 60)
            {
                minute++;
                seconds = 0;
            }
            UpdateTexts();
            yield return new WaitForSeconds(1f);
        }
    }
}
