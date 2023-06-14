using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ResetLevel();
    }
    void ResetLevel()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
