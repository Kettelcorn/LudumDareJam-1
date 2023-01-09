using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] private Button exit;
    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(Leave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Leave()
    {
        Debug.Log("You quit the game");
        Application.Quit();
    }
}

