using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with spike COLLISIONENTER");
            InvokeRepeating(nameof(Damage), 0.05f, 1f);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CancelInvoke(nameof(Damage));
    }

    private void Damage()
    {
        if (SceneController.Instance.gameOver) return;
        HealthManager.Instance.Damage(damage);
    }
}
