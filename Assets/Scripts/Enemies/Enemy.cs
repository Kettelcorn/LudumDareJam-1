using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int time;
    [SerializeField] private float speed;
    [SerializeField] private float pause;
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;
    private int counter;
    private bool death;
    private bool drag;
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
        if (death)
        {
            Death();
        }
        else
        {
            Movement();
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

    public void Movement()
    {
        counter++;
        if (counter < time) rb.velocity = new Vector2(speed, 0);
        else if (counter < time + pause) rb.velocity = new Vector2(0, 0);
        else if (counter < time * 2 + pause) rb.velocity = new Vector2(-speed, 0);
        else if (counter < time * 2 + pause * 2) rb.velocity = new Vector2(0, 0);
        else counter = 0;
    }
    

    public void Death()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hole"))
            Debug.Log("Is touching hole");
        if (collision.gameObject.CompareTag("Hole") && death && drag == false)
        {
            Debug.Log("should be burried");
            transform.position = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
        }
    }
}
