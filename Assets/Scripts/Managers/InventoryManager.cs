using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            
            Image[] itemSprites = itemObject.GetComponentsInChildren<Image>();

            foreach (Image iSprite in itemSprites)
            {
                if (iSprite.name == "ItemSprite")
                {
                    iSprite.sprite = item.itemSprite;
                    Debug.Log("Set image");
                }
            }
            /* 
            TextMeshProUGUI itemName = GetComponent<TextMeshProUGUI>();
            itemName.text = item.name;
            */
        }
    }
}