using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioControls : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] float additiveJumpForce;
    [SerializeField] float additiveJumpDuration;
    [SerializeField] float additiveJumpMaxDuration;

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] int jumpCount;
    [SerializeField] int jumpLimit;

    bool wallJumping;
    // Start is called before the first frame update
    void OnGUI() // Testing
    {
        GUI.Label(new Rect(10, 80, 300, 30), "JumpForce = " + jumpForce);
        GUI.Label(new Rect(10, 100, 300, 30), "JumpCount = " + jumpCount);
        GUI.Label(new Rect(10, 120, 300, 30), "JumpLimit = " + jumpLimit);
        GUI.Label(new Rect(10, 140, 300, 30), "AdditiveJumpDuration = " + additiveJumpDuration);
        GUI.Label(new Rect(10, 160, 300, 30), "AdditiveJumpMaxDuration = " + additiveJumpMaxDuration);
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
        // Regular Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= jumpLimit && wallJumping == false)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        // Additive Jump "Mario Jump"
        if (Input.GetKey(KeyCode.Space) && jumpCount <= jumpLimit && additiveJumpDuration <= additiveJumpMaxDuration && wallJumping == false)
        {
            additiveJumpDuration += Time.deltaTime;
            playerRb.AddForce(Vector2.up * additiveJumpForce * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpCount <= jumpLimit && wallJumping == false)
        {
            additiveJumpDuration = 0;
            jumpCount++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 0 && wallJumping == true)
        {
            jumpCount++;
            playerRb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
            Debug.Log("Wall Jump Force was" + (jumpForce * 2)); // Testing.
        }
    }
    void IncreaseJumpLimit()
    {
        jumpLimit++;
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
