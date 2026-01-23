using TMPro;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    private bool timerActive;
    private float currentTime;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        currentTime = 0f;
    }

    private void Start()
    {
        //StartTimer();
    }

    private void Update()
    {

        if (timerActive)
        {
            currentTime += Time.deltaTime;
        }
        else if (!timerActive && Mathf.Abs(rb.linearVelocity.x) > .1f)
        {
            StartTimer();
        }

            TimeSpan time = TimeSpan.FromSeconds(currentTime);

        text.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}
