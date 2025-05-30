using UnityEngine;

public class PillarSmall : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public Transform lightSource;
    public float detectionRange = 3f;
    public float attackRange = 1.5f;

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

        // Update internal state
        if (lightInFront) isAwake = true;
        if (!lightInFront) isAwake= false;

        anim.SetBool("IsAwake", isAwake);
    }

    public void BitePlayer()
    {
        Debug.Log("CHOMP!");
        // ins damage logic
    }
}
