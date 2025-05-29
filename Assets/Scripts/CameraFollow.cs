using UnityEngine;

/// <summary>
/// Smoothly follows a target (player) with offset.
/// Must be attatched to a camera object
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Transform of the target to follow the player.")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [Header("Offset from the player's position.")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Tooltip("Time for the camera to smoothly catch-up to the target.")]
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero; // Used internally by SmoothDamp

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera toward players position with damping
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
            );
    }
}
