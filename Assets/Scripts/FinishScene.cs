using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScene : MonoBehaviour
{
    public void StartOver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
