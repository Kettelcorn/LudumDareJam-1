using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Checks if player is hiding behind tree or dragging a farmer
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.GetComponent<Player>().Organs == null)
        {
            player.GetComponent<Player>().Hide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().Hide = false;
        }
    }

}
