using System.IO;
using TMPro;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;
    private string saveLocation;
    private InventoryManager inventoryManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    private void Update()
    {
        
    }

    public void SaveGame()
    {
        SceneController.Instance.SendGameUpdate("Game Saved");
        SaveData saveData = new SaveData()
        { 
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryManager.GetInventoryItems()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SceneController.Instance.SendGameUpdate("Loading Save");
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            // Stänger av det här temporarily för man börjar på fel ställe i vissa scener
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            inventoryManager.SetInventoryItems(saveData.inventorySaveData);
        }
        else
        {
            Debug.Log("no save? D:");
            SaveGame();
        }
    }
}
