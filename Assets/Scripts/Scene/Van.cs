using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Van : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private Text amount;
    [SerializeField] private AudioClip chaching;
    [SerializeField] private GameObject earned;

    private float timer;
    private int money;
    private GameObject store;
    private bool touch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    public bool Touch
    {
        get { return touch; }
    }

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
            if (money == 50)
            {
                SceneManager.LoadScene(sceneName: "Victory Scene");
            }
            Debug.Log("Successfully sold");
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
