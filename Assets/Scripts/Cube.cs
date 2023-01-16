using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool _isExplode;

    public Rigidbody2D RigidBody => _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        Ball.OnBallExplode += BallExplodeEffect;
    }

    private void OnDisable()
    {
        Ball.OnBallExplode -= BallExplodeEffect;
    }

    private void BallExplodeEffect(Ball ball)
    {
        if (!_isExplode)
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Rewind(transform);
                DOTween.Kill(transform);
            }

            var direction = ((Vector2)transform.position - (Vector2)ball.transform.position).normalized;
            transform.DOPunchPosition(direction, 0.2f, 10, 10f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DOTween.IsTweening(transform))
            {
                DOTween.Rewind(transform);
                DOTween.Kill(transform);
            }

            var direction = ((Vector2)transform.position - Vector2.zero).normalized;
            transform.DOPunchPosition(direction, 0.2f, 10, 10f);
        }
    }

    public void Explode()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector2.left * 200f);
        _collider.isTrigger = false;
        _isExplode = true;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("ExplodedCube");
        CubeCollector.instance.AddCube(this);
    }
}
