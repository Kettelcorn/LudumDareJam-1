using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private Button exit;
    
    // Add listener to button
    void Start()
    {
        exit.onClick.AddListener(Leave);
    }

    // Exit game
    void Leave()
    {
        Application.Quit();
    }
}

