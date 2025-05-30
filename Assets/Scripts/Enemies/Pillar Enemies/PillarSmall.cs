using UnityEngine;

public class PillarSmall : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public Transform lightSource;
    public float detectionRange = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    private Rigidbody2D rb;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAwake = false;

    void Update()
    {
        float distToLight = Vector2.Distance(transform.position, lightSource.position);
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        bool lightInFront = distToLight < detectionRange;
        bool playerInRange = isAwake && distToPlayer < attackRange;

        // Update Animator parameters
        anim.SetBool("LightInFront", lightInFront);
        anim.SetBool("PlayerInRange", playerInRange);

        isAwake = lightInFront;
        anim.SetBool("IsAwake", isAwake);
    
        if (playerInRange && Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    public void BitePlayer()
    {
        Debug.Log("CHOMP!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10, transform.position);
        }
    }
}
