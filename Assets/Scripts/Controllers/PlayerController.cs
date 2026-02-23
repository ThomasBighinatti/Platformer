using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
    
        //TODO empecher le joueur de rester collé au mur quand on se déplace dessus, mvt en l'air si besoin, limiter vitesse de chute
    
        private Vector2 _moveInput;
        private Rigidbody2D _rb;
    
    
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;

        [Header("Player")] [SerializeField] private float jumpStrength = 5f;
        [SerializeField] private bool jumpButtonPressed = false;
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private bool grounded = true;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float jumpCutMultiplier = 0.5f; // reduit la vitesse quand on relache le bouton saut
        [SerializeField] private float jumpBufferTime = 0.2f;
        private bool _jumpButtonReleased = false;

        [Header("Misc")] [SerializeField] private float groundCheckDistance = 0.5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.2f);

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                jumpButtonPressed = true;
                _jumpBufferCounter = jumpBufferTime;
            }
        
            if (ctx.canceled)
            {
                _jumpButtonReleased = true;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            //viser ? 
        }

        private void FixedUpdate()
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;

            //mvt
            if (grounded)
            {
                _rb.linearVelocity = new Vector2(_moveInput.x * playerSpeed, _rb.linearVelocity.y);
            }
            else //mvt en l'air
            {
                _rb.linearVelocity = new Vector2(_moveInput.x * playerSpeed, _rb.linearVelocity.y);
                //TODO
            }
        
            //coyoteTime
            if (grounded)
            {
                _coyoteTimeCounter = coyoteTime;
            }
            else
            {
                _coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate
            }
        
            //saut
            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpStrength);
                _coyoteTimeCounter = 0f;
                _jumpBufferCounter = 0f;
                jumpButtonPressed = false;
            }
        
            //jump cut
            if (_jumpButtonReleased)
            {
                if (_rb.linearVelocity.y > 0f)
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * jumpCutMultiplier);
                }
                _jumpButtonReleased = false;
            }
        
        
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            grounded = hit.collider;
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
        }
    }
}