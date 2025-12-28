using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<IOData> Items = new List<IOData>();
    [SerializeField] private Transform InventoryGrid;
    [SerializeField] private GameObject BaseInventoryItem;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        DontDestroyOnLoad(gameObject);

    }
    public void Add(IOData itemData) 
    {
        Items.Add(itemData);
    }
    public void LoadInventory()
    {
        foreach(Transform item in InventoryGrid)
        {
            Destroy(item.gameObject);
        }
        foreach(IOData item in Items)
        {
            GameObject itemObject = Instantiate(BaseInventoryItem, InventoryGrid);
            
            Image itemSprite = itemObject.GetComponent<Image>();
            itemSprite.sprite = item.itemSprite;

            TextMeshProUGUI itemName = GetComponent<TextMeshProUGUI>();
            itemName.text = item.name;
        }
    }
}