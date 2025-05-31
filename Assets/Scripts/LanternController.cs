using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class LanternController : MonoBehaviour
{
    private Animator anim;

    public GameObject lanternLightObject;
    public Light2D lanternLight;
    public float fadeDuration = 0.5f;

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

    void Start()
    {
        if (lanternLight != null)
            lanternLight.intensity = 0f;
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

    // Called by Animation Event early in the equip animation (not at the end)
    public void OnEquipStart()
    {
        lanternLightObject.SetActive(true);
        ShowLanternLight();  // starts fading light in gradually
    }


    // Called by Animation Event at the end of the equip animation
    public void OnEquipComplete()
    {
        lanternEquipped = true;

        if (lanternLightObject != null)
            lanternLightObject.SetActive(true);

        ShowLanternLight();
    }


    // Called by Animation Event at the end of unequip animation
    public void OnUnEquipComplete()
    {
        lanternEquipped = false;
        HideLanternLight();
    }


    public void ShowLanternLight()
    {
        if (lanternLightObject != null)
            StartCoroutine(FadeLight(1f));
    }

    public void HideLanternLight()
    {
        if (lanternLightObject != null)
            StartCoroutine(FadeLight(0f));
    }

    IEnumerator FadeLight(float targetIntensity)
    {
        float startIntensity = lanternLight.intensity;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            lanternLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / fadeDuration);
            yield return null;
        }

        lanternLight.intensity = targetIntensity;

        if (targetIntensity == 0f && lanternLightObject != null)
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
