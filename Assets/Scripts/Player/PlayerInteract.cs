using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private InventoryManager inventoryManager;

    [SerializeField] LayerMask warpObject;
    CinemachineImpulseSource impulseSource;
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    private void Update()
    {
        Collider2D contactWarpObject = Physics2D.OverlapCircle(transform.position, 2, warpObject);

        if (contactWarpObject)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;

            impulseSource = contactWarpObject.GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulseWithForce(0.19f);
            if(currentScene == 1)
            {
                SceneController.Instance.LoadScene(currentScene + 1);
            }
            else if (currentScene == 2)
                SceneController.Instance.LoadScene(currentScene - 1);

        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
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