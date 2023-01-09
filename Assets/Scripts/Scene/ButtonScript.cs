using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        Debug.Log("You pressed the button");
        SceneManager.LoadScene("Main Scene");
    }
}

    
