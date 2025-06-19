using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;

    public static InventorySystem Instance { get; set; }

    public List<GameObject> slotList = new List<GameObject>();
    public List<String> itemList = new List<string>();    

    public GameObject inventoryScreenUI;
    public bool isOpen;
    //public bool isFull;

    private GameObject AddItem;
    private GameObject SlotItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isOpen = false;
        populateSlotList();
        Cursor.visible = false;
    }

    private void populateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //SelectionManager.Intance.DisableSelection();
            //SelectionManager.Intance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //SelectionManager.Intance.EnableSelection();
                //SelectionManager.Intance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }

    public void AddToInventory(string ItemName)
    {
        SlotItem = FindNextSlot();

        AddItem = Instantiate(Resources.Load<GameObject>(ItemName), SlotItem.transform.position, SlotItem.transform.rotation);
        AddItem.transform.SetParent(SlotItem.transform);

        RecaculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    private GameObject FindNextSlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int Counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && Counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    Counter -= 1;
                }
            }
        }
    }

    public void RecaculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
                Debug.Log("Found item: " + result);
            }
        }
    }
}
