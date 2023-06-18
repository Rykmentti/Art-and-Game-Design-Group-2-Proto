using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // Reference to the player's transform
    public float smoothSpeed = 0.125f;     // The speed at which the camera will follow the player
    public Vector3 offset;       // The offset from the player's position

    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = smoothedPosition;
    }
}

