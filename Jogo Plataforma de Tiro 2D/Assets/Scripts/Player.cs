using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class Player : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed      = 2.5f;
    public float jumpForce  = 2f;


    [Header("Ground Properties")]
    public LayerMask groundLayer;
    public float groundDistance;
    public bool isGrounded;
    public Vector3[] footOffset;

    [Header("Audio")]
    [SerializeField] AudioCharacter audioPlayer = null;

    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;

    private Weapon weapon;

    private bool isJump;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private int direction = 1;
    private float originalXScale;
    private float xVelocity;
    private bool isFire;

    private LayerMask softGround;
    private LayerMask hardGround;
    private GroundType groundType;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Record the original x scale of the player
        originalXScale = transform.localScale.x;

        softGround = LayerMask.GetMask("Ground");
        hardGround = LayerMask.GetMask("GroundHard");
        col         = GetComponent<Collider2D>();
        weapon      = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        PhysicsCheck();

        if (isFire == false)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            movement = new Vector2(horizontal, 0);
            //If the sign of the velocity and direction don't match, flip the character
            if (xVelocity * direction < 0f)
                Flip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1") && isFire == false && isGrounded)
        {
            movement = Vector2.zero;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("fire");
        }

       
    }
    private void FixedUpdate()
    {
        if (!isFire)
        {
            xVelocity = movement.normalized.x * speed;
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }


        UpdateGround();
        // Play audio
        if (isGrounded)
            audioPlayer.PlaySteps(groundType, Mathf.Abs(xVelocity));


    }

    private void LateUpdate()
    {
        //animator.SetTrigger("die");

        animator.SetFloat("xVelocity", Mathf.Abs(xVelocity));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("fire"))
        {
            isFire = true;
        }
        else { isFire = false; }
       

    }

    public void Shoot()
    {
        if (weapon != null)
        {
            weapon.Shoot();
           
        }
    }

    private void PhysicsCheck()
    {

        isGrounded = false;

        leftCheck = Raycast(new Vector2(footOffset[0].x * direction, footOffset[0].y), Vector2.down, groundDistance, groundLayer);
        rightCheck = Raycast(new Vector2(footOffset[1].x * direction, footOffset[1].y), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || rightCheck)
        {
            isGrounded = true;
        }
    }

    private void Flip()
    {
        //Turn the character by flipping the direction
        direction *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = originalXScale * direction;

        //Apply the new scale
        transform.localScale = scale;
    }

    private void UpdateGround()
    {
        // Use character collider to check if touching ground layers
        if (col.IsTouchingLayers(softGround))
            groundType = GroundType.Soft;
        else if (col.IsTouchingLayers(hardGround))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;

    }



    public RaycastHit2D Raycast(Vector3 origin, Vector2 rayDirection, float length, LayerMask mask)
    {
        Vector3 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + origin, rayDirection, length, mask);
      
       
        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + origin, rayDirection * length, color);
        
     
        return hit;
    }


 
}
