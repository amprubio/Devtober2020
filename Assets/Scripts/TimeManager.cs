using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private const int DEGREES=6;

    public RectTransform min;
    public RectTransform hour;

    private int currmin=0;
    private int currhour = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHour(int h, int min)
    {
        currhour = h;
        currmin = min;
    }
    public void AddMin(int min)
    {
        currmin += min;

        if (currmin < 60)
        {
            currhour += currmin % 60;
            currmin = currmin / 60 * (currmin % 60);

        }
    }
}
