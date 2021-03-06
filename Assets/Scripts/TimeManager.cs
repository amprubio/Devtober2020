﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private const int DEGREES=6;

    public Transform min;
    public Transform hour;

    private int currmin=0;
    private int currhour = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (min == null || hour == null)
            Debug.LogError("manecillas nulas");
     
    }

    // Update is called once per frame
    void Update()
    {
        hour.localRotation = Quaternion.Euler(0, 0,(DEGREES * 5) * currhour);
        min.localRotation = Quaternion.Euler(0, 0,(DEGREES * currmin));
         
    }

    public void setHour(int h, int min)
    {
        currhour = h;
        currmin = min;
        print(currhour + ":" + currmin);
    }
    public void AddMin(int min)
    {
        currmin += min;

        if (currmin < 60|| currmin > -60)
        {
            AddHour(1);
            min = min - 60;
        }
        
    }
    public void AddHour(int h)
    {
        currhour += h;

    }
}

