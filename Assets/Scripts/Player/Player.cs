using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float dragSpeed;
    [SerializeField] private GameObject hole;

    private float tempSpeed;
    private GameObject[] enemy;
    private Rigidbody2D rb;
    private GameObject beingDragged;
    private Collider2D col;
    private float move;
    private bool hidden;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ignore collision with enemies
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject npc in enemy)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        
    }

    void Update()
    {

        // Sets horizontal movement
        move = Input.GetAxis("Horizontal");
        if (move > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } 
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (beingDragged != null)
        {
            rb.velocity = new Vector2(move * dragSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * move, rb.velocity.y);
        }
        
        

        if (Input.GetKeyDown(KeyCode.Space) && beingDragged == null)
        {
            tempSpeed = speed;
            speed = 0;
            Invoke("Dig", 1);
        }

        // If player presses F, attempt to kill npc
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();
        }

        // If player presses left shift, attempt to drag dead body
        if (Input.GetKeyDown(KeyCode.LeftShift) && ClosestEnemy() != null)
        {
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
        if (beingDragged != null)
        {
            if (beingDragged.GetComponent<Enemy>().Drag)
            {
                Drag();
            }
        }
           

    }

    public bool Hide
    {
        get { return hidden; }
        set { hidden = value; }
    }

    public GameObject Organs
    {
        get { return beingDragged; }
        set { beingDragged = value; }
    }

    // Selects closest npc, checks if it is close enough, and kills it if so
    private void Kill()
    {
        GameObject victim = ClosestEnemy(); 
        if (victim != null)
        {
            victim.GetComponent<Enemy>().Dead = true;
            victim.GetComponent<SpriteRenderer>().sortingLayerName = "Even more behind tree";
            victim.transform.Rotate(0, 0, 90);
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

    private void Dig()
    {
        if (col == null)
        {
            Instantiate(hole, new Vector2(transform.position.x, transform.position.y - 1.5f), transform.rotation);
        }
        else if (col.gameObject.CompareTag("Hole"))
        {
            Destroy(col.gameObject);
        }
        speed = tempSpeed;
    }

    // Checks all enemies and returns the closest one
    private GameObject ClosestEnemy()
    {
        int victim = 0;
        for (int i = 1; i < enemy.Length; i++)
        {
            if (Vector2.Distance(transform.position, enemy[i].transform.position) <
                Vector2.Distance(transform.position, enemy[victim].transform.position))
            {
                victim = i;
            }
            
        }
        Debug.Log("Victim =" + victim);
        Debug.Log("Distance: " + Vector2.Distance(transform.position, enemy[victim].transform.position));
        if ((Vector2.Distance(transform.position, enemy[victim].transform.position)) < 2)
        {
            Debug.Log("You are close enough");
            return enemy[victim];
        }
        return null;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        col = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        col = null;
    }
}
