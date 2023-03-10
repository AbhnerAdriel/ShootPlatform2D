using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Properties")]
    public float speed;
    public float attackDistance;

    [HideInInspector]
    public int direction;

    [Header("RayCast Properties")]
    public LayerMask layerGround;
    public float lengthGround;
    public float lengthWall;
    public Transform rayPointGround;
    public Transform rayPointWall;
    public RaycastHit2D hitGround;
    public RaycastHit2D hitWall;


    protected Animator animator;
    protected Rigidbody2D rb;

    protected Transform player;  //to locate the player in the game
    protected float playerDistance;

    protected virtual void Awake()
    {
        player      = FindObjectOfType<Player>().transform;
        animator    = GetComponent<Animator>();
        rb          = GetComponent<Rigidbody2D>();
        direction   = (int)transform.localScale.x;
    }

    protected virtual void Update()
    {
        GetDistancePlayer();
    }

    protected virtual void Flip()
    {
        direction *= -1;
        transform.localScale = new Vector2(direction, transform.localScale.y);
    }

    protected virtual RaycastHit2D RaycastGround()
    {
        //Send out the desired raycast and record the result
        hitGround = Physics2D.Raycast(rayPointGround.position, Vector2.down, lengthGround, layerGround);

        //...determine the color base on if the raycast hit...
        Color color = hitGround ? Color.red : Color.green;

        //...and draw the ray in the scene view
        Debug.DrawRay(rayPointGround.position, Vector2.down * lengthGround, color);

        //Return the result of the raycast
        return hitGround;
    }

    protected virtual RaycastHit2D RaycastWall()
    {
        //Send out the desired raycast and record the result
        hitWall = Physics2D.Raycast(rayPointWall.position, Vector2.right * direction, lengthWall, layerGround);

        //...determine the color base on if the raycast hit...
        Color color = hitWall ? Color.yellow : Color.blue;

        //...and draw the ray in the scene view
        Debug.DrawRay(rayPointWall.position, Vector2.right * direction * lengthWall, color);

        //Return the result of the raycast
        return hitWall;
    }

    protected void GetDistancePlayer() 
    {
        playerDistance = player.position.x - transform.position.x;
    }
}
