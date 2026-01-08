using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();

            if (item != null)
            {
                // add item to inventory
                bool itemAdded = inventoryManager.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}