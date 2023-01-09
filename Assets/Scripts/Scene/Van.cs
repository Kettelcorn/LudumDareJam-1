using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Van : MonoBehaviour
{
    [SerializeField] private GameObject earned;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip chaching;
    [SerializeField] private Text amount;

    private GameObject store;
    private float timer;
    private int money;
    private bool touch;

    // Return value for touch
    public bool Touch
    {
        get { return touch; }
    }

    // Update is called once per frame
    void Update()
    {
        // Timer for &10,000 object and destroy after 1 second
        if (timer >= 0)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                Destroy(store);
                timer = -1;
            }
        }
    }

    // Checks if player is colliding with van, and records score of player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touch = true;
        }
        if (collision.gameObject.CompareTag("Player") && player.GetComponent<Player>().Organs != null)
        {
            player.GetComponent<Player>().Organs.transform.position = new Vector2(10000000, 10000000);
            player.GetComponent<Player>().Organs.GetComponent<Enemy>().Drag = false;
            player.GetComponent<Player>().Organs = null;
            money += 10;
            if (money == 0)
            {
                amount.text = "$0";
            } 
            else
            {
                amount.text = "$" + money + ",000";
            }
            GetComponent<AudioSource>().clip = chaching;
            GetComponent<AudioSource>().Play();
            store = Instantiate(earned, new Vector2(transform.position.x - 1f, transform.position.y + 3f), transform.rotation);
            timer = 0;
            
            // Finish game if all 5 farmers are brought to van
            if (money == 50)
            {
                SceneManager.LoadScene(sceneName: "Victory Scene");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touch = false;
        }
    }
}
