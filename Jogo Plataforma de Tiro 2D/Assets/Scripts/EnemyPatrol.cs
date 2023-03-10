using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : Enemy
{

    [Header("Attack Properties")]
    public float timerWaitAttack;
    public float timerShootAttack;

    private bool idle;
    private bool die;
    private bool shoot;
    private Weapon weapon;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    protected override void Update()
    {
        base.Update();

        if (!RaycastGround().collider || RaycastWall().collider)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (CanAttack())
        {
            Attack();
        }
        else
        {
            Movement(); 
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("idle", idle);
    }

    private void Movement()
    {
        float horizontalVelocity = speed;
        horizontalVelocity = horizontalVelocity * direction;
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        idle = false;
    }

    private bool CanAttack()
    {
        return ((float)Mathf.Abs(playerDistance)) <= attackDistance;
    }

    private void Attack() 
    {
        StopMovement();
        DistancieFlipPlayer();
        CanShoot();
    }

    private void StopMovement() 
    {
        rb.velocity = Vector2.zero;
        idle = true;
    }

    private void DistancieFlipPlayer()
    {
        // player está a direita
        if (playerDistance >= 0 /*&& direction == -1*/) 
        {
            // inimigo está olhando para a esquerda, então precisa virar:
            if (direction == -1)
                Flip();
            
        } 
        else
        {
            // inimigo está olhando para a direita, então precisa virar:
            if (direction == 1)
                Flip();
        }
    }

    private void CanShoot()
    {
        if (!shoot)
            StartCoroutine("Shoot");
    }

    private IEnumerable Shoot()
    {
        shoot = true;

        yield return new WaitForSeconds(timerWaitAttack);

        AnimationShoot();

        yield return new WaitForSeconds(timerShootAttack);

        shoot = false;
    }

    private void AnimationShoot()
    {
        animator.SetTrigger("shoot");
    }

    private void ShootPreFab() 
    {
        if (weapon != null)
        {
            weapon.Shoot();
        }
    }

    public void Die()
    {
        // die = true;
        // rb.velocity = Vector2.zero;
        // animator.SetTrigger("die");
    }

    private void OnDisabled() 
    {
        Destroy(gameObject, 3f);
    }
}
