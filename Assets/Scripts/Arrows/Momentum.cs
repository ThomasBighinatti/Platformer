using Managers;
using UnityEngine;

namespace Arrows
{
    public class Momentum : Arrow
    {
        public override void StartArrow()
        {
            ArrowShot(ArrowManager.Instance.LookingTowards);
            ArrowManager.Instance.MomentumQueue.Enqueue(this);
        }

        protected override void FixedUpdate()
        {
            if (_recalling)
            {
                Vector2 target = ArrowManager.PlayerTransform.transform.position;
                Vector2 directionToPlayer = (target - (Vector2)transform.position);
                IsPlanted = false;
                Rb.constraints = RigidbodyConstraints2D.None;
                Rb.AddForce(directionToPlayer * recallStrength);
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

        protected override void ArrowShot(Vector2 direction)
        {
            base.ArrowShot(direction);
        }
        
        private bool _recalling;

        public void Recall()
        {
            _recalling = true;
        }
        
    }
}
