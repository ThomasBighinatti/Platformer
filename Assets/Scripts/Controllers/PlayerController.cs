using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Player")] [SerializeField] float jumpStrenght = 5f;
    [SerializeField] private bool jumpButtonPressed = false;
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] bool grounded = true;

    [Header("Misc")] [SerializeField] private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (grounded && ctx.started)
        {
            jumpButtonPressed = true;
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

        //saut
        if (jumpButtonPressed)
        {
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);
            jumpButtonPressed = false;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        grounded = hit.collider != null;

        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, grounded ? Color.green : Color.red);
    }
}
