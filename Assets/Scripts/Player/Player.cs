using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private GameObject[] enemy;

    private Rigidbody2D rb;
    private float move;
    private int jumpTracker;

    
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

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpTracker < 2)
        {
            jumpTracker++;
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
        }
        
        // If player presses F, attempt to kill npc
        if (Input.GetKeyDown(KeyCode.F))
        {
            Kill();  
        }

        // If player presses left shift, attempt to drag dead body
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Drag();
        }
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

    private void Drag()
    {
        GameObject victim = ClosestEnemy();
        if (victim != null && victim.GetComponent<Enemy>().Dead)
        {
            Debug.Log("You can drag this mf");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpTracker = 0;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("You are colliding with enemy");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (jumpTracker == 0)
        {
            jumpTracker++;
        }
    }
}
