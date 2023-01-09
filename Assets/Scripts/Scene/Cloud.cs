using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject van;
    [SerializeField] private GameObject fence;
    [SerializeField] private float speed;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       
    }

    // Update is called once per frame
    void Update()
    {
        float move = player.GetComponent<Player>().Move * 0.5f;
        if (!van.GetComponent<Van>().Touch && !fence.GetComponent<Fence>().Touch)
        {
            if (player.GetComponent<Player>().Organs == null)
            {
                rb.velocity = new Vector2(speed * move, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(speed * move / 3, rb.velocity.y);
            }
                
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
