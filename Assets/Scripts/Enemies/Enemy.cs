using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject police;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject text;
    [SerializeField] private Sprite hat;
    [SerializeField] private Sprite farmer;
    [SerializeField] private int time;
    [SerializeField] private float speed;
    [SerializeField] private float pause;

    private GameObject[] enemy;
    private Rigidbody2D rb;
    private float gameTimer;
    private bool death;
    private bool drag;
    private bool burried;

    // Getters and Setters
    public bool Dead
    {
        get { return death; }
        set { death = value; }
    }
    public bool Drag
    {
        get { return drag; }
        set { drag = value; }
    }
    public bool Bury
    {
        get { return burried; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Ignore collision with other farmers
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject npc in enemy)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;
        // If not dead, have normal movement
        if (!death)
        { 
            Movement();
        }

        // If police officer find the dead body not burried, end game
        if (Math.Abs(transform.position.x - police.transform.position.x) < 1.5 && death && !burried)
        {
            SceneManager.LoadScene(sceneName: "Game Over Found");
        }

        // Flips sprite based on direction you are moving
        if (rb.velocity.x > 0 && !death)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Default movement loop for farmer
    public void Movement()
    {
        if ((int)gameTimer % 4 == 0)
        {
            rb.velocity = new Vector2(speed, 0);
        } 
        else if ((int)gameTimer % 4 == 1)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if ((int)gameTimer % 4 == 2)
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    // Checks collision to see if farmer should be burried in hole
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hole") && death && drag == false)
        {
            GetComponent<Animator>().SetBool("Burried", true);
            transform.position = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - .1f);
            if (!burried)
            {
                transform.Rotate(0, 0, -90);
            }
            burried = true;  
        }
        else
        {
            GetComponent<Animator>().SetBool("Burried", false);
            GetComponent<SpriteRenderer>().sprite = farmer;
            if (burried)
            {
                transform.Rotate(0, 0, 90);
            }
            burried = false;
        }
    }
}
