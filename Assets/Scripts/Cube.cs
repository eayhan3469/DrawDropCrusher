using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool _isDropped;

    public Rigidbody2D RigidBody => _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void EffectFromBall()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector2.left * 200f);
        _collider.isTrigger = false;
        _isDropped = true;
        gameObject.tag = "Untagged";
    }

    private void Update()
    {
        if (_isDropped)
        {
            _rigidbody.AddForce((CubeCollector.instance.transform.position - transform.position).normalized * 1f);
        }
    }
}
