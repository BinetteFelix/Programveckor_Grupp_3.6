using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private ItemDictionary itemDictionary;

    [SerializeField] private Transform InventoryGrid;
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private GameObject BaseItemPrefab;
    public int slotCount;
    public List<GameObject> itemPrefabs = new List<GameObject>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in InventoryGrid)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
    }
    public List<InventorySaveData> GetInventoryItems()
    {
        List <InventorySaveData> invData = new List <InventorySaveData>();

        foreach (Transform slotTransform in InventoryGrid)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in InventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(SlotPrefab, InventoryGrid.transform);
        }
        foreach(InventorySaveData data in inventorySaveData)
        {
            if (data .slotIndex < slotCount)
            {
                Slot slot = InventoryGrid.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}