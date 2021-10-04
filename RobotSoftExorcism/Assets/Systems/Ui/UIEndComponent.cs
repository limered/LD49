using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndComponent : MonoBehaviour
{
    public GameObject Curtain;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Curtain.GetComponent<Animator>().Play("close_curtain");
        }
    }
}
