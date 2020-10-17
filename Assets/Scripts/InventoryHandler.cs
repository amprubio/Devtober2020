using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    Canvas invState;

    void Start()
    {
        if (GameObject.Find("Inventory_UI"))
            invState = GameObject.Find("Inventory_UI").GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory") && invState)
        {
            Inventory();
        }
    }

    public bool isRunning()
    {
        return invState;
    }

    public void Inventory()
    {
        if (invState.enabled == false)
        {
            invState.enabled = true;
        }
        else if (invState.enabled)
        {
            invState.enabled = false;
        }
    }
}
