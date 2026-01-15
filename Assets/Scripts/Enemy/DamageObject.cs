using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InvokeRepeating(nameof(Damage), 0f, 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(Damage));
    }

    private void Damage()
    {
        if (SceneController.Instance.gameOver) return;
        HealthManager.Instance.Damage(damage);
    }
}
