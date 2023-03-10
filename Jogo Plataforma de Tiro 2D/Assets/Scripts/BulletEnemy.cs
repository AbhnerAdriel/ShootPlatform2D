using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Bullet
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().AddDamage(damage);
            Explode();
            if (collision.GetComponent<Health>().health == 0)
            {
                //GameManager.isGameOver = true;
            }
        }
        if (collision.CompareTag("Ground"))
        {
            Explode();
        }


    }
}
