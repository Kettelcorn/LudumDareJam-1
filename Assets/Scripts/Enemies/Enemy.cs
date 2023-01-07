using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int time;
    [SerializeField] private float speed;
    [SerializeField] private float pause;
    private Rigidbody2D rb;
    private int counter;
    private bool death;
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

    void Movement()
    {
        counter++;
        if (counter < time) rb.velocity = new Vector2(speed, 0);
        else if (counter < time + pause) rb.velocity = new Vector2(0, 0);
        else if (counter < time * 2 + pause) rb.velocity = new Vector2(-speed, 0);
        else if (counter < time * 2 + pause * 2) rb.velocity = new Vector2(0, 0);
        else counter = 0;
    }
    

    void Death()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
