using System.Collections;
using Datas;
using UnityEngine;

namespace Arrows
{
    public abstract class Arrow : MonoBehaviour
    {
        
        [SerializeField] protected ArrowData data;

        [Header("To add to data")] 
        // serializefield temporaire qu'il faudra mettre par la suite dans le data
        
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
        protected bool IsPlanted;

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
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

        private IEnumerator WaitForDestroy()
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
        }

        public void SetDynamic() => Rb.bodyType = RigidbodyType2D.Dynamic;

    }
}
