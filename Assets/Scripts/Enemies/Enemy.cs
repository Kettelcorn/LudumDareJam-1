using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] private float speed;
    [SerializeField] private float pause;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject police;
    private Rigidbody2D rb;
    private int counter;
    private bool death;
    private bool drag;
    private bool burried;
    private GameObject[] enemy;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        counter = 0;
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject npc in enemy)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!death)
        { 
            Movement();
        }

        if (Math.Abs(transform.position.x - police.transform.position.x) < 1.5 && death && !burried)
        {
            SceneManager.LoadScene(sceneName: "Game Over Found");
        }

        if (rb.velocity.x > 0 && !death)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

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

    public void Movement()
    {
        counter++;
        if (counter < time) rb.velocity = new Vector2(speed, 0);
        else if (counter < time + pause) rb.velocity = new Vector2(0, 0);
        else if (counter < time * 2 + pause) rb.velocity = new Vector2(-speed, 0);
        else if (counter < time * 2 + pause * 2) rb.velocity = new Vector2(0, 0);
        else counter = 0;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hole"))
            Debug.Log("Is touching hole");
        if (collision.gameObject.CompareTag("Hole") && death && drag == false)
        {
            Debug.Log("should be burried");
            transform.position = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - .1f);
            burried = true;
        }
        else
        {
            burried = false;
        }
    }
}
