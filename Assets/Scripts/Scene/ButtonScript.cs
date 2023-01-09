using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Button button;
    
    // Create button listener
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);

    }

    // Start Game
    void TaskOnClick()
    {
        SceneManager.LoadScene("Main Scene");
    }
}

    
