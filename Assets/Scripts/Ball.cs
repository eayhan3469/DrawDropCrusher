using DG.Tweening;
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

    public static Action<Vector2> OnBallExplode;

    private Rigidbody2D _rigidbody;
    private Action<Ball> _killAction;
    private State _currentState;
    private Vector2 _launchPoint;
    private float _speedFactorToExplode = 1f;

    private enum State
    {
        OnAir,
        OnLine
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _currentState = State.OnAir;
    }

    private void Update()
    {
        if (_currentState == State.OnLine && Vector2.Distance((Vector2)transform.position, _launchPoint) > 0.5f)
        {
            _rigidbody.AddForce((_launchPoint - (Vector2)transform.position).normalized * _lineLaunchPower);
        }

        _speedFactorToExplode = _rigidbody.velocity.magnitude / 5f;
        _speedFactorToExplode = Math.Clamp(_speedFactorToExplode, 1f, 3f);
        _explodeArea.transform.localScale = Vector3.one * _speedFactorToExplode;
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
            Debug.Log("Hit to Cube");
            _explodeArea.SetActive(true);

            DOVirtual.DelayedCall(0.05f, () => OnBallExplode?.Invoke((Vector2)transform.position));
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
            cube.Explode();
            Debug.Log("Trigger To Cube");
        }
    }

    private IEnumerator DelayedKillAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        _killAction(this);
    }
}
