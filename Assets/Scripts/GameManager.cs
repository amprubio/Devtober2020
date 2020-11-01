﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public TimeManager time_manager_;
    public MapSystem map_system_;
    
    private int days;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        days = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetDays() { return days; }
    public void SetDays(int days_) { days = days_; }

    public void SetMapActive() { map_system_.SetActive(true); }
    public void SetMapUnactive() { map_system_.SetActive(false); }

}
