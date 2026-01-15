using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private InputAction interactAction;
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    private void Update()
    {
        if (InputManager.instance.interactAction.WasPressedThisFrame())
        {
            Collider2D contactWarpObject = Physics2D.OverlapCircle(transform.position, 2, 1);

            if (contactWarpObject)
            {
            }
        }
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