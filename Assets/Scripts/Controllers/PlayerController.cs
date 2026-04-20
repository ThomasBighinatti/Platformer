using System;
using Datas;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")] 
        [SerializeField] private PlayerData data;
        [Space(10f)] 
        
        [Header("To add to data")] 
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        [SerializeField] private bool stopVelocity;
        private static bool _stopVelocity;
        [Space(10f)]
        
        [Header("Visualisation")]
        [SerializeField] private bool grounded = false;
        [SerializeField] private bool onSlope = false;
        [Space(10f)]
        
        [Header("Slope Settings")]
        [SerializeField] private float slopeAngleThreshold = 5f;
        [SerializeField] private bool canJumpOnSlope = false;
        [Space(10f)]

        [Header("Raycast Settings")]
        [SerializeField] private float groundCheckDistance = 0.5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.2f);
        [SerializeField] private float boxCastCooldown = 0.1f;
        [Space(10f)]
        
        [Header("Friction")]
        [SerializeField] private PhysicsMaterial2D noFrictionMaterial;
        [SerializeField] private PhysicsMaterial2D frictionMaterial;
     
        private Vector2 _moveInput;
        private static Rigidbody2D _rb;
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
                _jumpBufferCounter = data.JumpBufferTime;
                _jumpButtonReleased = false;
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
            _rb.gravityScale = 0;
            _stopVelocity = stopVelocity;
        }
        
        private void FixedUpdate()
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;
            _boxCastCooldownCounter -= Time.fixedDeltaTime; //j'ai ecrit "=-" au lieu de "-="...

            _velocity = _rb.linearVelocity;
            
            _velocity = Movement(_velocity);
            _velocity = Jump(_velocity);
            _velocity = JumpCut(_velocity);
            _velocity = ApplyCustomGravity(_velocity);

            _velocity = new Vector2(
                Mathf.Clamp(_velocity.x, -data.MaxSpeed, data.MaxSpeed), 
                Mathf.Max(_velocity.y, -data.MaxFallSpeed)
            );

            _rb.linearVelocity = _velocity;
        }
        
        private Vector2 Movement(Vector2 targetVelocity)
        {
            float targetSpeedX = _moveInput.x * data.PlayerSpeed;
            
            RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            bool isFloorNormal = groundHit.collider is not null && groundHit.normal.y > 0.5f;
            grounded = isFloorNormal && _boxCastCooldownCounter <= 0f; //perso au sol si raycast + si le cooldown est a 0
            
            if (grounded)
            {
                _coyoteTimeCounter = data.CoyoteTime;
                
                #region MovementSlope
                float slopeAngle = Vector2.Angle(groundHit.normal, Vector2.up);

                if (slopeAngle > slopeAngleThreshold)
                {
                    onSlope = true; 
                    _playerCollider.sharedMaterial = noFrictionMaterial;
                    _rb.sharedMaterial = noFrictionMaterial;
                }
                else
                {
                    onSlope = false;
                    targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, data.PlayerAcceleration * Time.fixedDeltaTime);
    
                    _playerCollider.sharedMaterial = frictionMaterial;
                    _rb.sharedMaterial = frictionMaterial;
                }
                #endregion
            }
            else //mvt en l'air
            {
                _playerCollider.sharedMaterial = noFrictionMaterial;
                _rb.sharedMaterial = noFrictionMaterial;
                
                _coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, data.AirControl * Time.fixedDeltaTime);
            }

            return targetVelocity;
        }
        
        private Vector2 Jump(Vector2 targetVelocity)
        {
            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && !onSlope || _coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && canJumpOnSlope)
            {
                float jumpForce = (2f * data.JumpHeight) / data.TimeToJumpApex;
                targetVelocity.y = jumpForce;

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
                targetVelocity.y *= data.JumpCutMultiplier;
                _jumpButtonReleased = false; 
            }

            return targetVelocity;
        }

        private Vector2 ApplyCustomGravity(Vector2 targetVelocity)
        {
            if (grounded && !onSlope && targetVelocity.y <= 0.01f)
            {
                targetVelocity.y = -0.1f;
                return targetVelocity;
            }

            float gravity;

            // ca c'est pour ton temps de flotement en haut
            if (Mathf.Abs(targetVelocity.y) < data.ApexHangThreshold)
            {
                // en gros on fait ce que ta demandé le gd, on bricole ta gravité au sommet
                float apexGravity = (2f * data.JumpHeight) / Mathf.Pow(data.TimeToJumpApex, 2);
                gravity = apexGravity * data.ApexHangGravityMult;
            }
            else if (targetVelocity.y < 0)
            {
                gravity = (2f * data.JumpHeight) / Mathf.Pow(data.TimeToFall, 2);
            }
            else
            {
                gravity = (2f * data.JumpHeight) / Mathf.Pow(data.TimeToJumpApex, 2);
            }

            targetVelocity.y -= gravity * Time.fixedDeltaTime;
            return targetVelocity;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
        }

        public static void ActivateKnockback(Vector2 direction, float force)
        {
            if (_stopVelocity)
            {
                _rb.linearVelocity = Vector2.zero;
            }
            _rb.AddForce(force * direction, ForceMode2D.Impulse);
        }
        
    }
}