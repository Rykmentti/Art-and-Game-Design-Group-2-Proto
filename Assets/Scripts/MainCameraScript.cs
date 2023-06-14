using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float cameraScrollingSpeed;
    // Camera Script

    [SerializeField] Camera mainCamera; // Assigned in editor.
    float defaultHeight;
    float cameraOffset = 4f;

    void OnGUI() // Testing
    {
        GUI.Label(new Rect(10, 40, 300, 30), "Default Height = " + defaultHeight);
        GUI.Label(new Rect(10, 60, 300, 30), "Camera Offset = " + cameraOffset);
    }

    // Update is called once per frame
    void Update()
    {
        DebugCameraControls();

        transform.Translate(Vector2.right * cameraScrollingSpeed * Time.deltaTime);

        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);

        if (player.transform.position.y >= cameraOffset)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
        }
        if(player.transform.position.y <= -cameraOffset)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
        }

        
    }

    void DebugCameraControls()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            mainCamera.transform.position = new Vector3(175, 4, mainCamera.transform.position.z);
            cameraScrollingSpeed = 0.25f;
            player.transform.position = new Vector2(170, 2);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            cameraScrollingSpeed = cameraScrollingSpeed / 2;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            cameraScrollingSpeed = cameraScrollingSpeed * 2;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            player.transform.position = new Vector2(player.transform.position.x, 4);
        }
    }
}
