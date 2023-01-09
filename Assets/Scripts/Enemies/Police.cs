using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Police : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject music;
    [SerializeField] private GameObject exclaim;
    [SerializeField] private AudioClip slow;
    [SerializeField] private AudioClip fast;
    [SerializeField] private AudioClip alert;
    [SerializeField] private float speed;

    private GameObject currentExclaim;
    private Rigidbody2D rb;
    private float distance;
    private float timer;
    private float shakeTimer;
    private float exclaimTimer;
    private bool direction;
    private bool chase;
    private bool shake;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = 0.0f;
        music.GetComponent<AudioSource>().loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        distance = transform.position.x - player.transform.position.x;

        // Move exclamation mark on officer with him for 1 second, then destroy it and play fast music
        if (currentExclaim != null)
        {
            currentExclaim.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
            exclaimTimer += Time.deltaTime;
            if (exclaimTimer > 1)
            {
                Destroy(currentExclaim);
                currentExclaim = null;
                music.GetComponent<AudioSource>().clip = fast;
                music.GetComponent<AudioSource>().Play();
                music.GetComponent<AudioSource>().loop = true;
            } 
        }

        // Determines to chase or not
        if (Math.Abs(distance) < 8 &&  !player.GetComponent<Player>().Hide)
        {
            if (!chase)
            {
                currentExclaim = Instantiate(exclaim, new Vector2(transform.position.x, transform.position.y + 2.5f), transform.rotation);
                exclaimTimer = 0f;
                music.GetComponent<AudioSource>().Stop();
                music.GetComponent<AudioSource>().clip = alert;
                music.GetComponent<AudioSource>().Play();
            }
            chase = true;
            GetComponent<Animator>().speed = 2;
            shake = false;
        }

        // Changes layer of officer sprite depending on if the player has shaken the officer
        if (!shake)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Even more behind tree";
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Infront Tree";
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
            if (chase)
            {
                music.GetComponent<AudioSource>().clip = slow;
                music.GetComponent<AudioSource>().Play();
                music.GetComponent<AudioSource>().loop = true;
            }
            GetComponent<Animator>().speed = 1;
            chase = false;
            Movement(speed);
        }
        
        // If officer catches you, end the game
        if (Math.Abs(distance) < 1.6 && chase == true && shake == false)
        {
        SceneManager.LoadScene(sceneName: "Game Over Caught");
        }

        // Changes sprite orientation based on movement
        if (rb.velocity.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Sets default officer movement
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

    // Sets officer chase movement
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

    // Bounces officer off wall and send in other direction
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
