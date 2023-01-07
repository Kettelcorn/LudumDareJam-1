using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface EnemyInterface
{
    bool Drag
    {
        get;
        set;
    }
    bool Dead
    {
        get;
        set;
    }
    void Movement();
    void Death();
    void OnTriggerStay2D(Collider2D other);


}