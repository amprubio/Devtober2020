using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumOfDaysText : MonoBehaviour
{
    private Text numofdays;
    // Start is called before the first frame update
    void Start()
    {
        numofdays = this.GetComponent<Text>();
    }

    
    void Update()
    {
        int d = GameManager.instance.GetDays();

        numofdays.text = "day " + d;  
    }
}
