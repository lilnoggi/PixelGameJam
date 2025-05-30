using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LanternController : MonoBehaviour
{
    private Animator anim;

    public GameObject lanternLightObject;

    // State tracking
    private bool lanternEquipped = false;
    private bool isWalking = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        // safety check
        if (lanternLightObject != null)
            lanternLightObject.SetActive(false);
    }

    void Update()
    {
        HandleLanternToggle();
        UpdateMovementState();
        UpdateAnimatorParameters();
    }

    void UpdateAnimatorParameters()
    {
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("LanternEquipped", lanternEquipped);
    }

    /// <summary>
    /// Toggles lantern equipped state when player presses F.
    /// </summary>
    void HandleLanternToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lanternEquipped)
            {
                // Unequip lantern: play unequip animation then set state
                anim.SetTrigger("UnequipLantern");
            }
            else
            {
                // Equip lantern: play equip animation then set state
                anim.SetTrigger("EquipLantern");
            }
        }
    }

    // Called by Animation Event at the end of the equip animation
    public void OnEquipComplete()
    {
        lanternEquipped = true;
        if (lanternLightObject != null)
            lanternLightObject.SetActive(true);
    }

    // Called by Animation Event at the end of unequip animation
    public void OnUnEquipComplete()
    {
        lanternEquipped = false;
        if (lanternLightObject != null)
            lanternLightObject.SetActive(false);
    }

    /// <summary>
    /// Checks if player is moving to update walk/idle state.
    /// </summary>
    void UpdateMovementState()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        isWalking = Mathf.Abs(horizontalInput) > 0.1;
    }
}
