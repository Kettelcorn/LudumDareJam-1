using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            player.GetComponent<Player>().Organs.transform.position = new Vector2(500, 500);
            money += 1000;
            amount.text = "$" + money;
            Debug.Log("Successfully sold");
        }
    }
}
