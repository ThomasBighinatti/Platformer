using System.Collections.Generic;
using Controllers;
using Datas;
using Managers;
using UnityEngine;

namespace Arrows
{
    public class Momentum : Arrow
    {
        private MomentumArrowData MomentumData => data as MomentumArrowData;
        
        private float _recallSpeed;

        private bool _isOnStickyBlock;
        public void IsOnStickyBlock() => _isOnStickyBlock = true;

        protected override void StartArrow()
        {
            ArrowShot(ArrowManager.Instance.LookingTowards);
            hitCollider.enabled = false;
            hitCollider.enabled = true;
            ArrowManager.Instance.PushMomentumArrow(this);
        }

        private List<Vector2> _lastDirectionsToPlayer = new List<Vector2>();
        
        private Vector2 _directionToPlayer;
        private Vector2 DirectionToPlayer
        {
            get => _directionToPlayer;
            set
            {
                if (_directionToPlayer != Vector2.zero)
                {
                    _lastDirectionsToPlayer.Add(_directionToPlayer);
                }
                _directionToPlayer = value;
            }
        }
        
        protected override void FixedUpdate()
        {
            if (_recalling)
            {
                Vector2 target = ArrowManager.PlayerTransform.position;
                DirectionToPlayer = (target - (Vector2)transform.position).normalized;
                _recallSpeed += MomentumData.RecallAcceleration * Time.fixedDeltaTime;
                Rb.linearVelocity = DirectionToPlayer * _recallSpeed; 
                if (Vector2.Distance(transform.position, target) <= 2f)
                {
                    PlayerController.ActivateKnockback(_lastDirectionsToPlayer.Count >= 3 ? _lastDirectionsToPlayer[^3] : (target - _initialPositionOnRecall).normalized, 
                        _recallSpeed * MomentumData.KnockbackForce);
                    Destroy(gameObject);
                }
            }
            else if (CanUseGravity)
            {
                Rb.gravityScale = Mathf.Lerp(Rb.gravityScale, data.GravityForce, data.GravityLerpForce);
            }
            
            else
            {
                return;
            }

            if (IsPlanted)
                return;

            Vector3 direction = Rb.linearVelocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        private bool _recalling;
        private Vector2 _initialPositionOnRecall;

        public void Recall()
        {
            _recalling = true;
            IsPlanted = false;
            Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Rb.linearVelocity = Vector2.zero;
            _recallSpeed = MomentumData.RecallInitialSpeed;

            _initialPositionOnRecall = transform.position;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            if (_isOnStickyBlock)
            {
                RecallOnSticky();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (!IsPlanted || !_recalling)
                return;
            
            _recalling = false;
            
            if (data.UseDestroy)
            {
                StartCoroutine(WaitForDestroy());
            }
        }

        private void RecallOnSticky()
        {
            gameObject.layer = LayerMask.NameToLayer("ArrowNoSticky");
        }

        public override void DestroyArrow()
        {
            ArrowManager.Instance.PopMomentumArrow();
            base.DestroyArrow();
        }
    }
}
