using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    public static DeletePlayerPrefs Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

}
