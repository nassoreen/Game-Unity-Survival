using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public GameObject ItemInfoUi;

    public static InventorySystem Instance { get; set; }
    public GameObject ItemInfoUI { get; internal set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;


    //Pickup Popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;


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

        PopulateSlotList();

        Cursor.visible = false;
    }



    private void PopulateSlotList()
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

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            if(!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            
            isOpen = false;
        }
    }


    public void AddToInventory(string itemName)
    {

        whatSlotToEquip = FindNextEmptyslot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);


        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

    }


    // -------------- PopUpItem  --------------------//

    private Coroutine hidePopupCoroutine;

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        if (pickupAlert == null || pickupName == null || pickupImage == null)
        {
            Debug.LogWarning("Pickup UI elements are not assigned!");
            return;
        }

        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        // ��ش Coroutine ��͹˹�����ͻ�ͧ�ѹ��ë�͹�Ѻ
        if (hidePopupCoroutine != null)
        {
            StopCoroutine(hidePopupCoroutine);
        }

        hidePopupCoroutine = StartCoroutine(HidePickupAlertAfterDelay(1.5f));
    }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (pickupAlert != null)
        {
            pickupAlert.SetActive(false);
        }
    }

    private GameObject FindNextEmptyslot()
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

        int counter = amountToRemove;

        for (var i = slotList.Count - 1;i>=0;i--)
        {
            if (slotList[i].transform.childCount>0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter !=0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1; 
                }
            }
        }

        ReCalculeList();
        CraftingSystem.Instance.RefreshNeededItems();

    }


    public void ReCalculeList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {

                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

    internal void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string result = name.Replace("(Clone)", "");

                itemList.Add(result);
            }
        }
    }

}