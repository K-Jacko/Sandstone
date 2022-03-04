using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static Action OnMinChanged;

    public static Action OnHourChanged;
    
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    
    //Every (i)sec IRL is a Min in game
    private const float MinuteToRealTime = 5f;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        Hour = 0;
        Minute = 0;
        timer = MinuteToRealTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;
            OnMinChanged?.Invoke();
            if (Minute >= 60)
            {
                Hour++;
                OnHourChanged?.Invoke();
                Minute = 0;
            }

            timer = MinuteToRealTime;
        }
    }
}
