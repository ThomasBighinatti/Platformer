using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    
    //TODO empecher le joueur de rester collé au mur quand on se déplace dessus, mvt en l'air si besoin, limiter vitesse de chute
    
    private Vector2 moveInput;
    private Rigidbody2D rb;
    
    
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Player")] [SerializeField] float jumpStrenght = 5f;
    [SerializeField] private bool jumpButtonPressed = false;
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] bool grounded = true;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] private float jumpCutMultiplier = 0.2f; // reduit la vitesse quand on relache le bouton saut
    [SerializeField] private float jumpBufferTime = 0.2f;
    private bool jumpButtonReleased = false;

    [Header("Misc")] [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.2f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            jumpButtonPressed = true;
            jumpBufferCounter = jumpBufferTime;
        }
        
        if (ctx.canceled)
        {
            jumpButtonReleased = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //viser ? 
    }

    private void FixedUpdate()
    {
        jumpBufferCounter -= Time.fixedDeltaTime;

        //mvt
        if (grounded)
        {
            rb.linearVelocity = new Vector2(moveInput.x * playerSpeed, rb.linearVelocity.y);
        }
        else //mvt en l'air
        {
            rb.linearVelocity = new Vector2(moveInput.x * playerSpeed, rb.linearVelocity.y);
          //TODO
        }
        
        //coyoteTime
        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate
        }
        
        //saut
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrenght);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            jumpButtonPressed = false;
        }
        
         //jump cut
        if (jumpButtonReleased)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpButtonReleased = false;
        }
        
        
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
        grounded = hit.collider != null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
    }
}