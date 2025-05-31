using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Image lanternFill;

    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateLanternUI();
    }

    public void TakeDamage(int amount, Vector2 sourcePosition)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateLanternUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockback(sourcePosition);
        }
    }

    void ApplyKnockback(Vector2 sourcePosition)
    {
        if (isKnockedBack) return;

        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;
        StartCoroutine(KnockbackRoutine(direction));
    }

    System.Collections.IEnumerator KnockbackRoutine(Vector2 direction)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateLanternUI();
    }

    void UpdateLanternUI()
    {
        if (lanternFill != null)
        {
            lanternFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player is dead");
        // Death animation, restart level etc...
    }
}
