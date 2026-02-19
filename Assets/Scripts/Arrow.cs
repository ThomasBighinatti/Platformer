using System;
using System.Collections;
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
    
    private Rigidbody2D _rb;
    private float _gravityScale;
    private bool _canUseGravity;
    
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _gravityScale = _rb.gravityScale;
        
        ArrowShot(new Vector3(1,0));

        if (!useDestroy)
            return;
        StartCoroutine(WaitForDestroy());
    }

    private void FixedUpdate()
    {
        if (!_canUseGravity)
            return;
        _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, gravityForce, gravityLerpForce);
    }

    private void ArrowShot(Vector3 direction)
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
