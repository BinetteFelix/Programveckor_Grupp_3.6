using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageObject1 : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Movement player = other.GetComponent<Movement>();

        if (player != null)
            InvokeRepeating(nameof(Damage), 0.10f, 1f);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Movement player = other.GetComponent<Movement>();

        if (player != null)
            CancelInvoke(nameof(Damage));
    }
    private void Damage()
    {
        if (SceneController.Instance.gameOver) return;
        HealthManager.Instance.Damage(damage);
    }
}