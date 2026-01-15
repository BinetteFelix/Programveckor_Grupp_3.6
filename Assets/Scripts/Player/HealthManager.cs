using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    private int maxHealth = 4;
    private int health;

    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private Sprite filledHeartSprite;
    [SerializeField] private Image[] hearts;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = filledHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

        }
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //Trigger GameOver
            Debug.Log("Game over!");
            SceneController.Instance.TriggerGameOver();
            health = maxHealth;
        }
    }
}
