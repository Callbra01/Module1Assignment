using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Brandon Callaway | 000954560
public class RetryScreen : MonoBehaviour
{
    // Change scene to game start upon player pressing the spacebar
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
