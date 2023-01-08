using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hospital : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private Text amount;
    private int money;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.GetComponent<Player>().Organs != null)
        {
            player.GetComponent<Player>().Organs.transform.position = new Vector2(10000000, 10000000);
            player.GetComponent<Player>().Organs.GetComponent<Enemy>().Drag = false;
            player.GetComponent<Player>().Organs = null;
            money += 1000;
            amount.text = "$" + money;
            if (money == 5000)
            {
                SceneManager.LoadScene(sceneName: "Victory Scene");
            }
            Debug.Log("Successfully sold");
        }
    }
}
