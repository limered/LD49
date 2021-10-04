using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartComponent : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pressed p");
            SceneManager.LoadScene("BarScene");
        }
    }
}
