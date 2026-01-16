using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float LastAttackTime;
    [Range(0.5f, 5f)] public float AttackDelay;
    private bool canAttack;

    [SerializeField] private AudioClip[] slashSoundFXs;
    [SerializeField] Transform attackHitBox;
    [SerializeField] LayerMask enemyLayer;
    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        LastAttackTime -= Time.deltaTime;
        if (LastAttackTime < 0)
            canAttack = true;
        else
            canAttack = false;

        if (InputManager.instance.attackAction.WasPressedThisFrame() && canAttack)
        {
            LastAttackTime = AttackDelay;
            SoundFXManager.Instance.PlaySoundFXClip(slashSoundFXs, transform);
            animator.Play("AttackLeft");
            Physics2D.OverlapCircle(attackHitBox.transform.position, 2f, enemyLayer).transform.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
            if(enemy != null) enemy.TakeDamage(1);
        }
    }
}
