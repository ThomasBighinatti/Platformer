using System.Collections.Generic;
using Controllers;
using Datas;
using GPE;
using Managers;
using UnityEngine;

namespace Arrows
{
    public class Momentum : Arrow, IResettable
    {
        private MomentumArrowData MomentumData => data as MomentumArrowData;
        
        private float _recallSpeed;

        // y'a rien qui va niveau nomenclature, un fichier sur deux tes publics sont en majuscule en 1er,
        // les autres en _, des fois c'est pour les private, etc

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
                RecallAction();
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

            DirectionToAngle();
        }

        private void DirectionToAngle()
        {
            Vector3 direction = Rb.linearVelocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        private void RecallAction()
        {
            Vector2 target = LevelManager.Instance.Player.transform.position;
            DirectionToPlayer = (target - (Vector2)transform.position).normalized;
            _recallSpeed += MomentumData.RecallAcceleration * Time.fixedDeltaTime;
            Rb.linearVelocity = DirectionToPlayer * _recallSpeed;

            if (!(Vector2.Distance(transform.position, target) <= 2f)) 
                return;
            
            PlayerController.ActivateKnockback(_lastDirectionsToPlayer.Count >= 3 ? _lastDirectionsToPlayer[^3] : (target - _initialPositionOnRecall).normalized, 
                _recallSpeed * MomentumData.KnockbackForce);
            
            // dommage de pas les pooler
            Destroy(gameObject);
        }

        private bool _recalling;
        private bool _recalled;
        private Vector2 _initialPositionOnRecall;

        public void Recall()
        {
            if (_recalled) 
                return;
            
            _recalling = true;
            _recalled = true;
            IsPlanted = false;
            
            Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Rb.linearVelocity = Vector2.zero;
            _recallSpeed = MomentumData.RecallInitialSpeed;

            _initialPositionOnRecall= transform.position;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
            
            RecallForSticky();
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

        private void RecallForSticky()
        {
            gameObject.layer = LayerMask.NameToLayer("ArrowNoSticky");
        }
        
        public override void DestroyArrow()
        {
            if (isBeingDestroyed)
                return;
            
            if (!_recalled)
            {
                ArrowManager.Instance.PopMomentumArrow();
            }
            base.DestroyArrow();
        }
    }
}
