using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject _explodeArea;
    [SerializeField] private float _lineLaunchPower;

    private Rigidbody2D _rigidbody;
    private Action<Ball> _killAction;
    private State _currentState;
    private Vector2 _launchPoint;

    private enum State
    {
        OnAir,
        OnLine
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentState = State.OnAir;
    }

    private void Update()
    {
        if (_currentState == State.OnLine && Vector2.Distance((Vector2)transform.position, _launchPoint) > 0.5f)
        {
            _rigidbody.AddForce((_launchPoint - (Vector2)transform.position).normalized * _lineLaunchPower);
        }
    }

    public void InitKillAction(Action<Ball> killAction)
    {
        _killAction = killAction;
    }

    public void ResetValues()
    {
        gameObject.SetActive(false);
        _rigidbody.velocity = Vector2.zero;
        _explodeArea.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            _explodeArea.SetActive(true);
            StartCoroutine(DelayedKillAction(0.2f));
        }

        if (!collision.gameObject.CompareTag("Line") && !collision.gameObject.CompareTag("Cube"))
        {
            _killAction(this);
        }

        if (collision.gameObject.CompareTag("Line"))
        {
            _currentState = State.OnLine;
            _launchPoint = collision.gameObject.GetComponent<LineDrawer>().GetLaunchPoint();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            _currentState = State.OnAir;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//Explode area trigger controll
    {
        if (collision.CompareTag("Cube"))
        {
            var cube = collision.GetComponent<Cube>();
            cube.EffectFromBall();
        }
    }

    private IEnumerator DelayedKillAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        _killAction(this);
    }
}
