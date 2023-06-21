using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //"Mario" Controls
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip footsteps;
    [SerializeField] Animator animator; // Set in Editor

    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] float additiveJumpForce;
    [SerializeField] float additiveJumpDuration;
    [SerializeField] float additiveJumpMaxDuration;

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] int jumpCount;
    [SerializeField] int jumpLimit;

    [SerializeField] bool playeFootsteps;
    bool landing;
    bool wallJumping;
    bool jumping;
    // Start is called before the first frame update
    void OnGUI() // Testing
    {
        //GUI.Label(new Rect(10, 80, 300, 30), "JumpForce = " + jumpForce);
        //GUI.Label(new Rect(10, 100, 300, 30), "JumpCount = " + jumpCount);
        //GUI.Label(new Rect(10, 120, 300, 30), "JumpLimit = " + jumpLimit);
        //GUI.Label(new Rect(10, 140, 300, 30), "AdditiveJumpDuration = " + additiveJumpDuration);
        //GUI.Label(new Rect(10, 160, 300, 30), "AdditiveJumpMaxDuration = " + additiveJumpMaxDuration);
        //GUI.Label(new Rect(10, 180, 300, 30), "Jumping = " + jumping);
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
            if (!jumping)
            {
                StartCoroutine(PlayFootSteps());
                ResetAnimatorParameterValues();
                animator.SetBool("Running_Left", true);
            }
            if (jumping && !landing & !animator.GetBool("JumpStart_Left"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpFloat_Left", true);
            }
        }
        if (Input.GetKeyUp(KeyCode.A) && !jumping)
        {
            ResetAnimatorParameterValues();
            animator.SetBool("Idle_Left", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!jumping)
            {
                StartCoroutine(PlayFootSteps());
                ResetAnimatorParameterValues();
                animator.SetBool("Running_Right", true);
            }
            if (jumping && !landing && !animator.GetBool("JumpStart_Right"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpFloat_Right", true);
            }
        }
        if (Input.GetKeyUp(KeyCode.D) && !jumping)
        {
            ResetAnimatorParameterValues();
            animator.SetBool("Idle_Right", true);
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
            jumping = true;

            StartCoroutine(StartJumpAnimation());
        }
        // Additive Jump "Mario Jump"
        if (Input.GetKey(KeyCode.Space) && jumpCount <= jumpLimit && additiveJumpDuration <= additiveJumpMaxDuration && wallJumping == false)
        {
            additiveJumpDuration += Time.deltaTime;
            playerRb.AddForce(Vector2.up * additiveJumpForce * Time.deltaTime, ForceMode2D.Impulse);
        }
        // Wall Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 0 && wallJumping == true)
        {
            jumpCount++;
            playerRb.AddForce(Vector2.up * jumpForce * 2, ForceMode2D.Impulse);
            Debug.Log("Wall Jump Force was" + (jumpForce * 2)); // Testing.
        }
        IEnumerator StartJumpAnimation()
        {
            if (animator.GetBool("Running_Right") || animator.GetBool("Idle_Right"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpStart_Right", true);
            }
            if (animator.GetBool("Running_Left") || animator.GetBool("Idle_Left"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpStart_Left", true);
            }
            yield return new WaitForSeconds(0.25f);
            if (animator.GetBool("JumpStart_Right"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpFloat_Right", true);
            }
            if (animator.GetBool("JumpStart_Left"))
            {
                ResetAnimatorParameterValues();
                animator.SetBool("JumpFloat_Left", true);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpCount <= jumpLimit && wallJumping == false)
        {
            additiveJumpDuration = 0;
            jumpCount++;
        }
        IEnumerator PlayFootSteps()
        {
            if (playeFootsteps)
            {
                playeFootsteps = false;
                audioSource.PlayOneShot(footsteps, 0.3f);
                yield return new WaitForSeconds(0.4f);
                playeFootsteps = true;
            }
        }
    }
    void IncreaseJumpLimit() // Double Jump Power Up?
    {
        jumpLimit++;
    }

    void ResetAnimatorParameterValues() // Hope it works.
    {
        foreach (var parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
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
            StartCoroutine(LandingJumpAnimation());
            
            IEnumerator LandingJumpAnimation()
            {
                if (jumping && animator.GetBool("JumpFloat_Right"))
                {
                    landing = true;
                    ResetAnimatorParameterValues();
                    animator.SetBool("JumpEnd_Right", true);
                    yield return new WaitForSeconds(0.416f);
                    ResetAnimatorParameterValues();
                    animator.SetBool("Idle_Right", true);
                    landing = false;
                    jumping = false;
                }
                if (jumping && animator.GetBool("JumpFloat_Left"))
                {
                    landing = true;
                    ResetAnimatorParameterValues();
                    animator.SetBool("JumpEnd_Left", true);
                    yield return new WaitForSeconds(0.416f);
                    ResetAnimatorParameterValues();
                    animator.SetBool("Idle_Left", true);
                    landing = false;
                    jumping = false;
                }
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
