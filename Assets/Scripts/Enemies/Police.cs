using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Police : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject player;

    private Rigidbody2D rb;
    private bool direction;
    private bool chase;
    private bool shake;
    private float distance;
    private float timer;
    private float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        distance = transform.position.x - player.transform.position.x;

        if (Input.GetKeyDown(KeyCode.H))
            Debug.Log("Distance = " + distance);
        //Determines to chase or not
        if (Math.Abs(distance) < 8 &&  !player.GetComponent<Player>().Hide)
        {
            chase = true;
            shake = false;
        }

        //Flashes blue and red if chasing
        if (chase == true)
        {
            if ((int)timer % 2 == 0)
                GetComponent<SpriteRenderer>().color = Color.red;
            else
                GetComponent<SpriteRenderer>().color = Color.blue;

        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }

        //Determines to continue or end chase
        if (Math.Abs(distance) > 10)
        {
            shake = true;
        }

        if (chase == true && shake == false)
        {
            shakeTimer = timer;
            Chase();
        }
        else if (shake == true && timer - shakeTimer < 10)
        {
            Movement(speed * 2);
        }
        else
        {
            chase = false;
            Movement(speed);
        }
        
        if (Math.Abs(distance) < 1.6 && chase == true && shake == false)
        {
        SceneManager.LoadScene(sceneName: "Game Over Caught");
        }

        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Movement(float runSpeed)
    {
        if (direction)
        {
            rb.velocity = new Vector2(runSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-runSpeed, 0);
        }
    }

    private void Chase()
    {
        if (distance < 0)
        {
            direction = true;
            rb.velocity = new Vector2(speed * 2, 0);
        }
        else
        {
            direction = false;
            rb.velocity = new Vector2(-speed * 2, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = true;
        } 
        else if (collision.gameObject.CompareTag("Hospital"))
        {
            direction = false;
        }
    }
}
