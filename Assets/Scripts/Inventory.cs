using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemClass> itemDatabase = new List<ItemClass>();
    private readonly ItemClass[] storage = new ItemClass[10];

    GameObject contentUI;
    GameObject buttonUI;

    Text textButton;

    int storageIndex;
    private void Start()
    {
        contentUI = GameObject.Find("Content_UI");
        buttonUI = contentUI.transform.Find("Button").gameObject;

        textButton = buttonUI.GetComponentInChildren<Text>();
    }
    public void AddItem(int itemID)
    {
        foreach(var ItemClass in itemDatabase)
        {
            if (ItemClass.id == itemID)
            {
                Debug.Log(itemDatabase[itemID].name + " añadido al inventario");

                buttonUI.SetActive(true);

                storage[storageIndex] = ItemClass;
                textButton = buttonUI.GetComponentInChildren<Text>();
                textButton.text = ItemClass.name;
                buttonUI = Instantiate(buttonUI, contentUI.transform);

                buttonUI.SetActive(false);

                storageIndex += 1;
                return;
            }
        }
    }
    public void RemoveItem(int itemID)
    {
        foreach (var ItemClass in itemDatabase)
        {
            if (ItemClass.id == itemID)
            {
                storage[0] = null;
            }
        }
    }
}
