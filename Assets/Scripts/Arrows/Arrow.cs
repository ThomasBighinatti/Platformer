using System;
using System.Collections;
using Controllers;
using Datas;
using UnityEngine;

namespace Arrows
{
    public class Arrow : MonoBehaviour
    {

        
        [Header("Player Settings")] 
        [SerializeField] private ArrowData data;
        [Space(10f)]
        
        [Header("To add to data")]
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        [Space(10f)]
        
        [SerializeField] private GameObject player;


        private bool _canStartMoving;
        public bool CanStartMoving
        {
            get => _canStartMoving;
            set
            {
                _canStartMoving = value;
                if (CanStartMoving)
                {
                    StartArrow();
                }
            }
        }
    
        private Rigidbody2D _rb;
        private bool _canUseGravity;
        private bool _isPlanted;

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void StartArrow()
        {
            ArrowShot(WeaponController.Direction);

            if (!data.UseDestroy)
                return;
            StartCoroutine(WaitForDestroy());
        }

        private void FixedUpdate()
        {
            if (_recalling)
            {
                Vector2 target = WeaponController.Player.transform.position;
                Vector2 directionToPlayer= (target - (Vector2)transform.position);
                _isPlanted = false;
                _rb.constraints = RigidbodyConstraints2D.None;
                _rb.AddForce(directionToPlayer * 10);
            }
            else if (_canUseGravity)
            {
                _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, data.GravityForce, data.GravityLerpForce);
            }
            else
            {
                return;
            }
            
            if (_isPlanted)
                return;
        
            Vector3 direction = _rb.linearVelocity;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0,0, angle);
        }

        private void ArrowShot(Vector2 direction)
        {
            _rb.AddForce(direction * data.Strength * 10);
            if (data.UseGravity)
            {
                StartCoroutine(WaitForGravity());
            }
        }

        private IEnumerator WaitForGravity()
        {
            yield return new WaitForSeconds(data.GravityActivationTime);
            _canUseGravity = true;
        }

        private IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(data.DestroyTime);
            if (_recalling)
            {
                WeaponController.MomentumArrowShot.Dequeue();
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _isPlanted = true;
        }

        private bool _recalling;

        public void Recall()
        {
            _recalling = true;
        }
    }
}
