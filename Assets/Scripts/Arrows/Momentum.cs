using Controllers;
using Managers;
using UnityEngine;

namespace Arrows
{
    public class Momentum : Arrow
    {
        public float recallInitialSpeed = 200f;
        public float recallAcceleration = 200f;
        private float _recallSpeed;
        
        public override void StartArrow()
        {
            ArrowShot(ArrowManager.Instance.LookingTowards);
            ArrowManager.Instance.EnqueueArrow(this);
        }

        protected override void FixedUpdate()
        {
            if (Recalling)
            {
                Vector2 target = ArrowManager.PlayerTransform.position;
                Vector2 directionToPlayer = (target - (Vector2)transform.position).normalized;
                _recallSpeed += recallAcceleration * Time.fixedDeltaTime;
                Debug.Log(_recallSpeed);
                Rb.linearVelocity = directionToPlayer * _recallSpeed; 
                if (Vector2.Distance(transform.position, target) <= 1)
                {
                    PlayerController.ActivateKnockback(directionToPlayer,_recallSpeed);
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
            transform.eulerAngles = new Vector3(0,0, angle);
            
        }

        private bool _recalling;
        public bool Recalling
        {
            get => _recalling;
            set
            {
                _recalling = value;
                if (!_recalling) 
                    return;
                
                IsPlanted = false;
                Rb.constraints = RigidbodyConstraints2D.None;
                Rb.linearVelocity = Vector2.zero;
                _recallSpeed = recallInitialSpeed;
            } 
        }

        public void Recall()
        {
            Recalling = true;
        }
    }
}
