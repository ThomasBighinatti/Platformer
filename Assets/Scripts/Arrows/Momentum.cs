using Controllers;
using Managers;
using UnityEngine;

namespace Arrows
{
    public class Momentum : Arrow
    {
        [SerializeField] private float recallInitialSpeed = 200f;
        [SerializeField] private float recallAcceleration = 200f;
        private float _recallSpeed;

        protected override void StartArrow()
        {
            ArrowShot(ArrowManager.Instance.LookingTowards);
            ArrowManager.Instance.EnqueueMomentumArrow(this);
        }

        protected override void FixedUpdate()
        {
            if (_recalling)
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
            else
            {
                if (CanUseGravity)
                {
                    Rb.gravityScale = Mathf.Lerp(Rb.gravityScale, data.GravityForce, data.GravityLerpForce);
                }

                if (IsPlanted)
                    return;

                Vector3 direction = Rb.linearVelocity;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            
        }

        private bool _recalling;

        public void Recall()
        {
            _recalling = true;
            IsPlanted = false;
            Rb.constraints = RigidbodyConstraints2D.None;
            Rb.linearVelocity = Vector2.zero;
            _recallSpeed = recallInitialSpeed;
        }
    }
}
