using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
    
        //TODO réorganiser tout le bordel dans fixed update ,mvt en l'air si besoin, limiter vitesse de chute, saut en 3 phases
        
        [Header("Player Settings")] 
        [SerializeField] private float jumpStrength = 5f;
        [SerializeField] private float playerAcceleration = 5f;
        [SerializeField] private float playerSpeed = 5f;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float minSpeed = 3f;
        [SerializeField] private float coyoteTime = 0.1f;
        [SerializeField] private float jumpCutMultiplier = 0.5f;
        [SerializeField] private float jumpBufferTime = 0.2f;
        [SerializeField] private float boxCastCooldown = 0.1f;
        [Space(10f)]
        
        [Header("Visualisation")]
        [SerializeField] private bool grounded = true;
        [Space(10f)]
        
        [Header("Raycast Settings")]
        [SerializeField] private float groundCheckDistance = 0.5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.2f);
        [Space(10f)]
        
        [Header("Friction")]
        [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
        [SerializeField] private PhysicsMaterial2D frictionMaterial;
     
        private Vector2 _moveInput;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _playerCollider;
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;
        private float _boxCastCooldownCounter = 0;
        private bool _jumpButtonReleased = false;
        
        private Vector2 _velocity;
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
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
        
        
        private void Start()
        {
            _playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;
            _boxCastCooldownCounter -= Time.fixedDeltaTime; //j'ai ecrit "=-" au lieu de "-="...

            _velocity = _rb.linearVelocity;
            _velocity = Movement(_velocity);
            _velocity = Jump(_velocity);
            _velocity = JumpCut(_velocity);

            _rb.linearVelocity = _velocity;
        }
        
        private Vector2 Movement(Vector2 targetVelocity)
        {
            RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            grounded = groundHit.collider is not null && _boxCastCooldownCounter <= 0f; //perso au sol si raycast + si le cooldown est a 0
            
            if (grounded)
            {
                _playerCollider.sharedMaterial = frictionMaterial;
                _rb.sharedMaterial = frictionMaterial;
                _coyoteTimeCounter = coyoteTime;

                #region MovementSlope
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector2.up, groundHit.normal);
                targetVelocity = slopeRotation * new Vector2(_moveInput.x * playerSpeed, 0f);
                /* if(speed < maxSpeed)
                    {
                     speed += acceleration * Time.deltaTime;
                    }
                transform.position.x = transform.position.x + speed*Time.deltaTime;
                
                faire un truc comme ca mais partout en gros
                    */
                // Debug.Log(slopeRotation);
                #endregion
                
                if (slopeRotation != new Quaternion(0, 0, 0, 1))
                {
                    _rb.bodyType = RigidbodyType2D.Kinematic;
                    _rb.linearVelocity = Vector2.zero;
                }
            }
            else //mvt en l'air
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _playerCollider.sharedMaterial = noFrictionMaterial;
                _rb.sharedMaterial = noFrictionMaterial;
                targetVelocity = new Vector2(_moveInput.x * playerSpeed, targetVelocity.y);
                _coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate
            }

            return targetVelocity;
        }
        
        private Vector2 Jump(Vector2 targetVelocity)
        {
            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
            {
                targetVelocity = new Vector2(targetVelocity.x, jumpStrength);
                _coyoteTimeCounter = 0f;
                _jumpBufferCounter = 0f;
                _boxCastCooldownCounter = boxCastCooldown;
            }

            return targetVelocity;
        }
        
        private Vector2 JumpCut(Vector2 targetVelocity)
        {
            if (!_jumpButtonReleased) 
                return targetVelocity;
            
            if (targetVelocity.y > 0f)
            {
                targetVelocity = new Vector2(targetVelocity.x, targetVelocity.y * jumpCutMultiplier);
            }
            _jumpButtonReleased = false;

            return targetVelocity;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
        }
    }
}