using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            Vector2 attackPos = transform.root.position;
            health.TakeDamage(10, attackPos);
        }
    }
}
