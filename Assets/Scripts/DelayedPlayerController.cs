using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedPlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float inputLag;

    bool lagging;
    bool laggingRight;
    bool laggingLeft;

    // Start is called before the first frame update
    void OnGUI() // Testing
    {
        //GUI.Label(new Rect(10, 80, 300, 30), "InputLag = " + inputLag);
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputLag += Time.deltaTime / 5;

        DelayedPlayerControls();
    }
    void DelayedPlayerControls()
    {
        if (!lagging)
        {
            if (Input.GetKey(KeyCode.A)) // Move Left.
            {
                inputLag += Time.deltaTime / 5;
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                inputLag += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                lagging = true;
                laggingLeft = true;
            }
            if (Input.GetKey(KeyCode.D)) // Move Right.
            {
                inputLag += Time.deltaTime / 5;
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                inputLag += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                lagging = true;
                laggingRight = true;
            }
        }

        if (lagging)
        {
            if (laggingRight == true && inputLag >= 0)
            {
                inputLag -= Time.deltaTime;
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            if (laggingRight == true && inputLag <= 0)
            {
                lagging = false;
                laggingRight = false;
            }
            if (laggingLeft == true && inputLag >= 0)
            {
                inputLag -= Time.deltaTime;
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
            if (laggingLeft == true && inputLag <= 0)
            {
                lagging = false;
                laggingLeft = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) // Jump Up.
        {
            playerRb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }
}
