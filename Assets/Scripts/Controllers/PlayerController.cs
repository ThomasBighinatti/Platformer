using Datas;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Controllers
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        
        [Header("Player Settings")] 
        [SerializeField] private PlayerData data;
        [Space(10f)] 
        
        [Header("To add to data")] 
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        [SerializeField] private bool stopVelocity;
        [SerializeField] private float jumpSlopeAngle;
        private static float _stickyMult = 1;
        private static bool _onStickyCanJump = true;
        [Space(10f)]
        
        [Header("Visualisation")]
        [SerializeField] private bool grounded;
        [SerializeField] private bool onSlope;
        [Space(10f)]
        
        [Header("Slope Settings")]
        [SerializeField] private float slopeAngleThreshold = 5f;
        [SerializeField] private bool canJumpOnSlope = true;
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
        [Space(10f)]
        
        [Header("Animation")]
        [SerializeField] private PlayerAnimController playerAnimController;
        [SerializeField] private SpriteRenderer visual;
        
        private Vector2 _moveInput;
        private bool _lookingRight;
        
        private static Rigidbody2D _rb;
        private CapsuleCollider2D _playerCollider;
        
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;
        private float _boxCastCooldownCounter;
        
        private bool _jumpButtonReleased;
        
        private float _slopeDirection;
        private Vector2 _jumpSlopeVector;
        
        private static bool _stopVelocity;
        private static bool _isKnockedBack;

        private void Awake()
        {
            _playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rb.gravityScale = 0;
            _stopVelocity = stopVelocity;
            if (data is null)
            {
                Debug.LogError("No Data Entered");
            }
            _explosionAnimator = explosionAnimator;
            _explosionAnimator.gameObject.SetActive(false);

        }

        private void FixedUpdate()
        {
            _jumpBufferCounter -= Time.fixedDeltaTime;
            _boxCastCooldownCounter -= Time.fixedDeltaTime; //j'ai ecrit "=-" au lieu de "-="... // ha bah bravo

            Vector2 velocity = _rb.linearVelocity;
            
            velocity = Movement(velocity);
            velocity = Jump(velocity);
            velocity = JumpCut(velocity);
            velocity = ApplyCustomGravity(velocity);
            velocity = RestrictVelocity(velocity);
            
            FlipTowardsDirection(velocity);
            AnimationStateChange(velocity);

            _rb.linearVelocity = velocity;
        }

        public static void OnSticky(float newValue, bool reset)
        {
            if (reset)
            {
                _stickyMult = 1;
                _onStickyCanJump = true;
                return;
            }
            _stickyMult = newValue;
            _onStickyCanJump = false;
            
        }

        #region Movement
        
        private Vector2 Movement(Vector2 targetVelocity)
        {
            float targetSpeedX = _moveInput.x * data.PlayerSpeed * _stickyMult;
            
            RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
            
            bool isValidGround = groundHit.collider is not null;
            if (isValidGround && groundHit.collider.usedByEffector)
            {
                float playerBottom = _playerCollider.bounds.min.y;
                float platformTop = groundHit.collider.bounds.max.y;
        
                if (playerBottom < platformTop - 0.05f)
                {
                    isValidGround = false; 
                }
            }
            bool isFloorNormal = isValidGround && groundHit.normal.y > 0.5f;

            grounded = isFloorNormal && _boxCastCooldownCounter <= 0f; //perso au sol si raycast + si le cooldown est a 0
            
            if (grounded)
            {
                _coyoteTimeCounter = data.CoyoteTime;
                
                #region MovementSlope
                
                float slopeAngle = Vector2.Angle(groundHit.normal, Vector2.up);
                if (slopeAngle > slopeAngleThreshold)
                {
                    onSlope = true;
                    _slopeDirection = Mathf.Sign(groundHit.normal.x);
                    
                    ChangeToMaterial(noFrictionMaterial);
                }
                else
                {
                    onSlope = false;
                    targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, data.PlayerAcceleration * Time.fixedDeltaTime);
    
                    ChangeToMaterial(frictionMaterial);
                }
                
                #endregion
            }
            else //mvt en l'air
            {
                ChangeToMaterial(noFrictionMaterial);
                
                _coyoteTimeCounter -= Time.fixedDeltaTime; //fixeddeltatime prcq on est dans fixedupdate

                switch (_isKnockedBack)
                {
                    case true when _moveInput.x == 0:
                        return targetVelocity;
                    
                    case true when Mathf.Sign(_rb.linearVelocity.x) != Mathf.Sign(targetSpeedX) || Mathf.Abs(_rb.linearVelocity.x) - Mathf.Abs(targetSpeedX) <= 0:
                        targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, data.AirControl * Time.fixedDeltaTime);
                        _isKnockedBack = false;
                        break;
                    
                    case true:
                        return targetVelocity;
                    
                    default:
                        targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, targetSpeedX, data.AirControl * Time.fixedDeltaTime);
                        _isKnockedBack = false;
                        break;
                }
            }
            return targetVelocity;
        }

        private void ChangeToMaterial(PhysicsMaterial2D material)
        {
            _playerCollider.sharedMaterial = material;
            _rb.sharedMaterial = material;
        }
        
        #endregion
        
        #region Jump
        
        private bool _isPlayerJumping;
        private Vector2 Jump(Vector2 targetVelocity)
        {
            bool canJump = _coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && _onStickyCanJump;
            if (!canJump || (onSlope && !canJumpOnSlope)) 
                return targetVelocity;
            
            float jumpForce = 2f * data.JumpHeight / data.TimeToJumpApex;
            switch (onSlope)
            {
                case false:
                    ChangeToMaterial(noFrictionMaterial);
                    targetVelocity.y = jumpForce;
                    break;
                    
                case true:
                    float jumpSlopeRadians = jumpSlopeAngle * Mathf.Deg2Rad;
                    _jumpSlopeVector = new Vector2(Mathf.Sin(jumpSlopeRadians), Mathf.Cos(jumpSlopeRadians));
                    Vector2 jumpForceVector = _jumpSlopeVector * jumpForce;
                    jumpForceVector.x *= _slopeDirection;
                        
                    targetVelocity = jumpForceVector;
                    break;
            }
            
            _isPlayerJumping = true;
            grounded = false;

            _coyoteTimeCounter = 0f;
            _jumpBufferCounter = 0f;
            _boxCastCooldownCounter = boxCastCooldown;
                
            playerAnimController.SetJumpContact();
            SoundManager.Instance.SoundPlay(SoundManager.MainSfx.StartJump);

            return targetVelocity;
        }
        
        private Vector2 RestrictVelocity(Vector2 velocity)
        {
            velocity = new Vector2(
                Mathf.Clamp(velocity.x, -data.MaxSpeed, data.MaxSpeed), 
                Mathf.Max(velocity.y, -data.MaxFallSpeed)
            );
            return velocity;
        }
        
        private Vector2 JumpCut(Vector2 targetVelocity)
        {
            if (!_isPlayerJumping || !_jumpButtonReleased || !(targetVelocity.y > 0f)) 
                return targetVelocity;

            targetVelocity.y *= data.JumpCutMultiplier;
            _jumpButtonReleased = false;
            _isPlayerJumping = false;

            return targetVelocity;
        }

        private Vector2 ApplyCustomGravity(Vector2 targetVelocity)
        {
            
            if (grounded || targetVelocity.y <= 0f)
            {
                _isPlayerJumping = false;
                _jumpButtonReleased = false; 
            }
            
            if (grounded && !onSlope && targetVelocity.y <= 0.01f)
            {
                targetVelocity.y = -0.1f;
                return targetVelocity;
            }

            float gravity;

            // ca c'est pour ton temps de flottement en haut
            if (Mathf.Abs(targetVelocity.y) < data.ApexHangThreshold)
            {
                // en gros on fait ce que ta demandé le gd, on bricole ta gravité au sommet
                float apexGravity = 2f * data.JumpHeight / Mathf.Pow(data.TimeToJumpApex, 2);
                gravity = apexGravity * data.ApexHangGravityMult;
                playerAnimController.SetJumpFloat();
            }
            else if (targetVelocity.y < 0)
            {
                gravity = 2f * data.JumpHeight / Mathf.Pow(data.TimeToFall, 2);
                playerAnimController.SetJumpFall();
            }
            else
            {
                gravity = 2f * data.JumpHeight / Mathf.Pow(data.TimeToJumpApex, 2);
                playerAnimController.SetJumpRise();
            }

            targetVelocity.y -= gravity * Time.fixedDeltaTime;
            return targetVelocity;
        }
        
        #endregion

        #region InputActions
        
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
        
        #endregion

        #region Visual
        
        private void AnimationStateChange(Vector2 velocity)
        {
            playerAnimController.ChangeToJumpState = false;
            
            bool isJumping = playerAnimController.GetJump();
            
            if (isJumping && grounded && !onSlope)
            {
                playerAnimController.SetLand();
                playerAnimController.landed = true;
                SoundManager.Instance.SoundPlay(SoundManager.MainSfx.EndJump);
            }
            else if (!onSlope && !grounded)
            {
                playerAnimController.SetJump();
            }
            else if (onSlope && grounded && velocity.y <= 0)
            {
                playerAnimController.SetSlide();
            }
            else if (Mathf.Abs(velocity.x) < 0.01f && grounded && !playerAnimController.landed)
            {
                playerAnimController.SetIdle();
            }
            else if (grounded && !playerAnimController.landed)
            {
                playerAnimController.SetWalk();
            }
        }
        
        private void FlipTowardsDirection(Vector2 velocity)
        {
            _lookingRight = velocity.x switch
            {
                < 0 => true,
                > 0 => false,
                _ => _lookingRight
            };

            visual.flipX = _lookingRight;
        }
        
        #endregion

        [SerializeField] private Animator explosionAnimator;
        private static Animator _explosionAnimator;

        public static void ActivateKnockback(Vector2 direction, float force)
        {
            if (_stopVelocity)
            {
                _rb.linearVelocity = Vector2.zero;
            }

            _rb.AddForce(force * direction, ForceMode2D.Impulse);
            
            _explosionAnimator.gameObject.SetActive(true);
            PlayExplosionAnim(force);
            
            _isKnockedBack = true;
        }

        private static void PlayExplosionAnim(float force)
        {
            switch (force)
            {
                case < 24:
                    _explosionAnimator.Play("Impact1Anim", 0, 0f);
                    break;
                case < 26:
                    _explosionAnimator.Play("Impact2Anim", 0, 0f);
                    break;
                case < 28:
                    _explosionAnimator.Play("Impact3Anim", 0, 0f);
                    break;
                case >= 28:
                    _explosionAnimator.Play("Impact4Anim", 0, 0f);
                    break;
            }
        }
        
        public void DeactivateExplosionAnimator()
        {
            _explosionAnimator.gameObject.SetActive(false);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckDistance, boxSize);
        }
        
    }
}