using System;
using System.Collections;
using Controllers;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [Header("Arrow Settings")]
    [SerializeField] private float strength = 10f;
    [SerializeField] private bool useGravity = true;
    [SerializeField] private float gravityActivationTime = 1f;
    [SerializeField] private float gravityForce = 0.3f;
    [SerializeField] private float gravityLerpForce = 0.25f;
    [SerializeField] private bool useDestroy = true;
    [SerializeField] private float destroyTime = 10f;

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
    
    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void StartArrow()
    {
        ArrowShot(WeaponController.Direction);

        if (!useDestroy)
            return;
        StartCoroutine(WaitForDestroy());
    }

    private void FixedUpdate()
    {
        
        if (!_canUseGravity)
            return;
        _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, gravityForce, gravityLerpForce);
        
        Vector3 direction = _rb.linearVelocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0, angle);
    }

    private void ArrowShot(Vector2 direction)
    {
        _rb.AddForce(direction * strength * 10);
        StartCoroutine(WaitForGravity());
    }

    private IEnumerator WaitForGravity()
    {
        yield return new WaitForSeconds(gravityActivationTime);
        _canUseGravity = true;
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
