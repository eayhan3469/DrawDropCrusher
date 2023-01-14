using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject _explodeArea;

    private Rigidbody2D _rigidbody;
    private Action<Ball> _killAction;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Action<Ball> killAction)
    {
        _killAction = killAction;
    }

    public void ResetValues()
    {
        gameObject.SetActive(false);
        _rigidbody.velocity = Vector2.zero;
        _explodeArea.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cube"))
        {
            var cube = collision.GetComponent<Cube>();
            cube.EffectFromBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            _explodeArea.SetActive(true);
            StartCoroutine(DelayedKillAction(0.2f));
        }

        Debug.Log(collision.gameObject.tag);
        if (!collision.gameObject.CompareTag("Line") && !collision.gameObject.CompareTag("Cube"))
        {
            _killAction(this);
        }
    }

    private IEnumerator DelayedKillAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        _killAction(this);
    }
}
