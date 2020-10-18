using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Transform player;
    private Inventory inventory;

    public int index;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            inventory = player.GetComponent<Inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= 0.5f)
        {
            inventory.AddItem(index);
            Destroy(gameObject);
        }
    }
}
