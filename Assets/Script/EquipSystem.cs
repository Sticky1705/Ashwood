using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public GameObject numberHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject toolsHolder;
    public GameObject selectedItemModel;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
    }

    void SelectQuickSlot(int numb)
    {
        if (checkIfSlotFull(numb) == true)
        {
            selectedNumber = numb;

            if (selectedItem != null)
            {
                InventoryItem invItem = selectedItem.GetComponent<InventoryItem>();
                if (invItem != null)
                {
                    invItem.isSelected = false;
                }
            }

            selectedItem = GetSelectedItem(numb);

            if (selectedItem != null)
            {
                InventoryItem invItem = selectedItem.GetComponent<InventoryItem>();
                if (invItem != null)
                {
                    invItem.isSelected = true;
                }
                else
                {
                    Debug.LogWarning("Selected item is missing InventoryItem component: " + selectedItem.name);
                }
            }

            SetEquippedModel(selectedItem);

            // Reset all slot text colors to white
            foreach (Transform child in numberHolder.transform)
            {
                Transform textTransform = child.Find("Text");
                if (textTransform != null)
                {
                    Text text = textTransform.GetComponent<Text>();
                    if (text != null)
                    {
                        text.color = Color.white;
                    }
                }
            }

            // Highlight selected number in blue
            Transform numberTransform = numberHolder.transform.Find("number" + numb);
            if (numberTransform != null)
            {
                Transform textTransform = numberTransform.Find("Text");
                if (textTransform != null)
                {
                    Text toBeChanged = textTransform.GetComponent<Text>();
                    if (toBeChanged != null)
                    {
                        toBeChanged.color = Color.blue;
                    }
                    else
                    {
                        Debug.LogWarning("Missing Text component on: " + textTransform.name);
                    }
                }
                else
                {
                    Debug.LogWarning("Missing 'Text' child under: " + numberTransform.name);
                }
            }
            else
            {
                Debug.LogWarning("Couldn't find number holder child: number" + numb);
            }
        }
        else
        {
            selectedNumber = -1;

            if (selectedItem != null)
            {
                selectedItem.GetComponent<InventoryItem>().isSelected = false;
            }

            if (selectedItemModel != null)
            {
                DestroyImmediate(selectedItemModel.gameObject);
                selectedItemModel = null;
            }

            foreach (Transform child in numberHolder.transform)
            {
                Transform textTransform = child.Find("Text");
                if (textTransform != null)
                {
                    Text text = textTransform.GetComponent<Text>();
                    if (text != null)
                    {
                        text.color = Color.white;
                    }
                }
            }
        }
    }

    private void SetEquippedModel(GameObject SelectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItem = SelectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItem + "_Model"),
            new Vector3(0.384f, -0.258f, 0.858f), Quaternion.Euler(0, 98.731f, 0));
        selectedItemModel.transform.SetParent(toolsHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        int index = slotNumber - 1;

        if (index >= 0 && index < quickSlotsList.Count)
        {
            if (quickSlotsList[index].transform.childCount > 0)
            {
                return quickSlotsList[index].transform.GetChild(0).gameObject;
            }
        }

        return null;
    }

    bool checkIfSlotFull(int slotNumb)
    {
        int index = slotNumb - 1;

        if (index >= 0 && index < quickSlotsList.Count)
        {
            return quickSlotsList[index].transform.childCount > 0;
        }

        Debug.LogWarning("Quick slot index out of bounds: " + index);
        return false;
    }

    private void Start()
    {
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);

        InventorySystem.Instance.RecaculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
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

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
