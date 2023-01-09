using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject hole;
    [SerializeField] private GameObject shovel;
    [SerializeField] private float speed;
    [SerializeField] private float dragSpeed;
    [SerializeField] private float frameRate;
    

    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip clip3;
    
    private Rigidbody2D rb;
    private GameObject[] enemy;
    private GameObject beingDragged;
    private GameObject tempShovel;
    private Collider2D col;
    private float move;
    private float tempSpeed;
    private bool hidden;

    // Getters and Setters for private variables
    public GameObject TempShovel
    {
        get { return tempShovel; }
    }
    public GameObject Organs
    {
        get { return beingDragged; }
        set { beingDragged = value; }
    }
    public float Move
    {
        get { return move; }
    }
    public bool Hide
    {
        get { return hidden; }
        set { hidden = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().speed = frameRate;

        // Ignore collision with enemies
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject npc in enemy)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Sets horizontal movement, orients sprite to face right direction
        move = Input.GetAxis("Horizontal");
        if (move > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } 
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // Changes speed if dragging body
        if (beingDragged != null)
        {
            rb.velocity = new Vector2(move * dragSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * move, rb.velocity.y);
        }
        
        // Dig hole if not dragging body
        if (Input.GetKeyDown(KeyCode.Space) && beingDragged == null && tempShovel == null)
        {
            tempSpeed = speed;
            speed = 0;
            tempShovel = Instantiate(shovel, new Vector2(transform.position.x - 1f, transform.position.y), transform.rotation);

            // Delay instantiation of hole sprite
            Invoke("Dig", 1);
        }

        // If press F, attempt to kill
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();
        }

        // If player presses left shift, attempt to drag dead body
        if (Input.GetKeyDown(KeyCode.LeftShift) && ClosestEnemy() != null)
        {
            // Checks if closest enemy is dead and if they are being dragged
            if (ClosestEnemy().GetComponent<Enemy>().Dead && !ClosestEnemy().GetComponent<Enemy>().Drag)
            {
                beingDragged = ClosestEnemy();
                beingDragged.GetComponent<Enemy>().Drag = true;
            } 
            else if (ClosestEnemy().GetComponent<Enemy>().Dead && ClosestEnemy().GetComponent<Enemy>().Drag)
            {
                beingDragged.GetComponent<Enemy>().Drag = false;
                beingDragged = null;
            }
        }
        // If you are dragging someone, call Drag to set their position
        if (beingDragged != null)
        {
            if (beingDragged.GetComponent<Enemy>().Drag)
            {
                Drag();
            }
        }
    }

    

    // Selects closest farmer, checks if it is close enough, and kills it if so
    private void Kill()
    {
        GameObject victim = ClosestEnemy(); 
        if (victim != null)
        {
            if (victim.GetComponent<Enemy>().Dead == false)
            {
                
                victim.transform.Rotate(0, 0, 90);
                victim.GetComponent<Animator>().SetBool("Dead", true);
                AudioClip[] sound = { clip1, clip2, clip3};
                GetComponent<AudioSource>().clip = sound[Random.Range(0, 3)];
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().loop = false;

            }
            victim.GetComponent<Enemy>().Dead = true;
            victim.GetComponent<SpriteRenderer>().sortingLayerName = "Even more behind tree"; 
        }
    }

    // Player grabs dead enemy and drags, orienting position of enemy based on movement
    private void Drag()
    {
        GameObject victim = beingDragged;
        if (victim != null && victim.GetComponent<Enemy>().Dead)
        {
            if (move > 0)
            {
                victim.transform.position = new Vector2(transform.position.x - 1.5f, victim.transform.position.y);
            }
            else if (move < 0)
            {
                victim.transform.position = new Vector2(transform.position.x + 1.5f, victim.transform.position.y);
            }  
            else
                victim.transform.position = new Vector2(victim.transform.position.x, victim.transform.position.y);
        }
    } 

    // Destroy shovel sprite and instantiate hole object, return player movement to normal
    private void Dig()
    {
        Destroy(tempShovel);
        if (col == null)
        {
            Instantiate(hole, new Vector2(transform.position.x, transform.position.y - 1.5f), transform.rotation);
        }
        speed = tempSpeed;
    }

    // Checks all farmers and returns the closest one, and sees if they are close enough to perform action on
    private GameObject ClosestEnemy()
    {
        // Finds closest enemy
        int victim = 0;
        for (int i = 1; i < enemy.Length; i++)
        {
            if (Vector2.Distance(transform.position, enemy[i].transform.position) <
                Vector2.Distance(transform.position, enemy[victim].transform.position))
            {
                victim = i;
            }  
        }

        // Determines if they are close enough
        if ((Vector2.Distance(transform.position, enemy[victim].transform.position)) < 2)
        {
            return enemy[victim];
        }
        return null;
    }

    // For checking collision triggers
    private void OnTriggerStay2D(Collider2D collision)
    {
        col = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        col = null;
    }
}
