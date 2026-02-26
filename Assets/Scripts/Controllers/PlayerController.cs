using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
    
        //TODO empecher le joueur de rester collé au mur quand on se déplace dessus, mvt en l'air si besoin, limiter vitesse de chute
    
        private Vector2 _moveInput;
        private Rigidbody2D _rb;
        private CapsuleCollider2D playerCollider;
        private Quaternion slopeRotation;
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
        [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
        [SerializeField] private PhysicsMaterial2D frictionMaterial;
        

        private void Start()
        {
            playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                jumpButtonPressed = true;
                _jumpBufferCounter = jumpBufferTime;
            }
        
            if (context.canceled)
            {
                _jumpButtonReleased = true;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        

        private void FixedUpdate()
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;

            //mvt
            if (grounded)
            {
                playerCollider.sharedMaterial = frictionMaterial;
                _rb.sharedMaterial = frictionMaterial;
                Debug.Log(_rb.sharedMaterial);
                _rb.linearVelocity = new Vector2(_moveInput.x * playerSpeed, _rb.linearVelocity.y);
                _coyoteTimeCounter = coyoteTime;
            }
            else //mvt en l'air
            {
                playerCollider.sharedMaterial = noFrictionMaterial;
                _rb.sharedMaterial = noFrictionMaterial;
                Debug.Log(_rb.sharedMaterial);
                _rb.linearVelocity = new Vector2(_moveInput.x * playerSpeed, _rb.linearVelocity.y);
                _coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate
                //TODO
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
        
        
            RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            grounded = groundHit.collider;
            /*
            var ray = new Ray(transform.position, Vector3.down);

            if (Physics2D.Raycast(ray, out RaycastHit hitInfo, 0.2f, groundLayer))
            {
                slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                Vector3 rotatedVector = new Vector3(slopeRotation * _moveInput );
            }
            */
        }
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
        }
    }
}