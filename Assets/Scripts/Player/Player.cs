using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject hole;

    private GameObject[] enemy;
    private Rigidbody2D rb;
    private GameObject beingDragged;
    private Collision2D col;
    private float move;
    private bool drag;

    
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
        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            if (!col.gameObject.CompareTag("Hole"))
            {
                Instantiate(hole, new Vector2(transform.position.x, transform.position.y - 1.17f), transform.rotation);
            }
            else
            {
                Debug.Log("Should destroy");
                Destroy(col.gameObject);
            }
        }

        // If player presses F, attempt to kill npc
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();  
        }

        // If player presses left shift, attempt to drag dead body
        if (Input.GetKeyDown(KeyCode.LeftShift) && ClosestEnemy() != null)
        {
            if (ClosestEnemy().GetComponent<Enemy>().Dead)
            {
                drag = !drag; 
                beingDragged = ClosestEnemy();
            } 
            else if (beingDragged != null)
            {
                drag = !drag;
                beingDragged = null;
            }
        }

        if (drag)
        {
            Drag();
        }    

    }

    public bool Dragging
    {
        get{return drag; }
        set{ drag = value; }
        
    }


    // Selects closest npc, checks if it is close enough, and kills it if so
    private void Kill()
    {
        GameObject victim = ClosestEnemy(); 
        if (victim != null)
        {
            victim.GetComponent<Enemy>().Dead = true;
        }
    }

    // Player grabs dead enemy and drags, orienting position of enemy based on movement
    private void Drag()
    {
        GameObject victim = beingDragged;
        if (victim != null && victim.GetComponent<Enemy>().Dead)
        {
            if (move > 0)
                victim.transform.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
            else if (move < 0)
                victim.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
            else
                victim.transform.position = new Vector2(victim.transform.position.x, transform.position.y);

        }
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        col = collision;
    }
}
