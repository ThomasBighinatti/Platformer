using System.Collections;
using Datas;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Arrows
{
    public abstract class Arrow : MonoBehaviour
    {
        
        [SerializeField] protected ArrowData data;

        [Header("To add to data")] 
        [SerializeField] protected Collider2D hitCollider;
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        private Vector2 _arrowPosition;
        
        private bool _canStartMoving;
        public bool CanStartMoving
        {
            get => _canStartMoving;
            set
            {
                _canStartMoving = value;
                if (_canStartMoving)
                {
                    StartArrow();
                }
            }
        }
    
        protected Rigidbody2D Rb;
        protected bool CanUseGravity;
        public bool IsPlanted { get; protected set; }
        
        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        protected abstract void StartArrow();
        /*if (!data.UseDestroy)
                return;
            StartCoroutine(WaitForDestroy());*/

        protected abstract void FixedUpdate();

        protected virtual void ArrowShot(Vector2 direction)
        {
            Rb.AddForce(direction * data.Strength);
            if (data.UseGravity)
            {
                StartCoroutine(WaitForGravity());
            }
        }

        private IEnumerator WaitForGravity()
        {
            yield return new WaitForSeconds(data.GravityActivationTime);
            CanUseGravity = true;
        }

        protected IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(data.DestroyTime);
            Destroy(gameObject);
        }
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!CanStartMoving)
                return;
            
            Rb.constraints = RigidbodyConstraints2D.FreezeAll;
            IsPlanted = true;
            
            Tilemap hitMap = other.GetComponent<Tilemap>();
    
            if (hitMap != null)
            {
                Vector2 hitPoint = other.ClosestPoint(transform.position);
                Vector2 flightDirection = Rb.linearVelocity.normalized; 
                hitPoint += flightDirection * 0.1f;
                
                TileBase touchedTile = TileManager.Instance.GetTileType(hitPoint, hitMap);
                
                if (touchedTile != null)
                {
                    TileManager.Instance.SpawnParticleForTile(touchedTile, hitPoint);
                }
                
            }
            else
            {
                Debug.Log("No tilemap found");
            }
        }

        public void SetDynamic() => Rb.bodyType = RigidbodyType2D.Dynamic;

        
    }
}
