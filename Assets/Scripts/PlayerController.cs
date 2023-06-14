using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float boostedJumpForce;

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] int jumpCount;

    bool wallJumping;
    // Start is called before the first frame update
    void OnGUI() // Testing
    {
        GUI.Label(new Rect(10, 80, 300, 30), "Boosted Jump Force = " + boostedJumpForce);
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControls();
    }

    void PlayerControls()
    {
        // Regular Movement
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        // Sprint Movement
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed * 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5;
        }
        // Boosted Jump
        if (Input.GetKey(KeyCode.Space) && jumpCount <= 1 && wallJumping == false)
        {
            boostedJumpForce += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpCount <= 1 && wallJumping == false)
        {
            jumpCount++; // Double Jump Mechanic.
            playerRb.AddForce(Vector2.up * (boostedJumpForce * 2 + jumpForce), ForceMode2D.Impulse);
            Debug.Log("Boosted Jump Force was" + (boostedJumpForce * 2 + jumpForce)); // Testing.
            boostedJumpForce = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1 && wallJumping == true)
        {
            jumpCount++;
            playerRb.AddForce(Vector2.up * (jumpForce * 2), ForceMode2D.Impulse);
            Debug.Log("Wall Jump Force was" + (jumpForce * 2)); // Testing.
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundPlatform"))
        {
            jumpCount = 0;
            if (wallJumping)
            {
                wallJumping = false;
                speed = speed / 2;
                Debug.Log("Not Wall Jumping!");
            }          
        }

        if (other.gameObject.CompareTag("WallJumpPlatform"))
        {
            jumpCount = 0;
            if (!wallJumping)
            {
                wallJumping = true;
                speed = speed * 2;
                Debug.Log("Wall Jumping!");
            }
        }
    }
}
