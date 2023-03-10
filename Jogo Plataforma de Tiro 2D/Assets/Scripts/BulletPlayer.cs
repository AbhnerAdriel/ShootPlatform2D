using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : Bullet
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
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().AddDamage(damage);
            Explode();

            if (collision.GetComponent<Health>().health == 0)
            {
                collision.GetComponentInParent<EnemyPatrol>().Die();
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //}

}
