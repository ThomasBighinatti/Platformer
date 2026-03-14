using System;
using Datas;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
    
        /*TODO mvt en l'air si besoin,
         faire glisser sur les slopes,
         rendre la deceleration moins degueulasse,
         
         */
        
        [Header("Player Settings")] 
        [SerializeField] private PlayerData data;
        public PlayerDataWrapper RuntimeData;
        [Space(10f)]
        
        [Header("To add to data")]
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        [Space(10f)]
        
        [Header("Visualisation")]
        [SerializeField] private bool grounded = true;
        [SerializeField] private bool onSlope = false;
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
        private Rigidbody2D _rb;
        private CapsuleCollider2D _playerCollider;
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;
        private float _boxCastCooldownCounter = 0;
        private bool _jumpButtonReleased = false;
        
        private Vector2 _velocity;

        private void Awake()
        {
            RuntimeData = data.GetRuntimeData();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _jumpBufferCounter = RuntimeData.JumpBufferTime;
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
            _velocity = new Vector2(Mathf.Clamp(_velocity.x, -RuntimeData.MaxSpeed, RuntimeData.MaxSpeed), Mathf.Max(_velocity.y, RuntimeData.MaxFallSpeed));

            _rb.linearVelocity = _velocity;
        }
        
        private Vector2 Movement(Vector2 targetVelocity)
        {
            float targetSpeedX = _moveInput.x * RuntimeData.PlayerSpeed;
            
            RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            grounded = groundHit.collider is not null && _boxCastCooldownCounter <= 0f; //perso au sol si raycast + si le cooldown est a 0
            
            if (grounded)
            {
                _coyoteTimeCounter = RuntimeData.CoyoteTime;
                
                #region MovementSlope
                Quaternion slopeRotation = Quaternion.FromToRotation(Vector2.up, groundHit.normal);
                if (slopeRotation != new Quaternion(0, 0, 0, 1))
                {
                    onSlope = true; 
                    _playerCollider.sharedMaterial = noFrictionMaterial;
                    _rb.sharedMaterial = noFrictionMaterial;
                }
                else
                {
                    onSlope = false;
                    targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, RuntimeData.PlayerAcceleration * Time.fixedDeltaTime);
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
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, RuntimeData.AirControl * Time.fixedDeltaTime);
            }

            return targetVelocity;
        }
        
        private Vector2 Jump(Vector2 targetVelocity)
        {
            if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && onSlope == false)
            {
                targetVelocity = new Vector2(targetVelocity.x, RuntimeData.JumpStrength);
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
                targetVelocity = new Vector2(targetVelocity.x, targetVelocity.y * RuntimeData.JumpCutMultiplier);
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