using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private float _spawnRate;
    [SerializeField] private bool _usePool;

    private ObjectPool<Ball> _ballPool;

    private void Start()
    {
        _ballPool = new ObjectPool<Ball>(() => { return Instantiate(_ball); },
        b => { b.gameObject.SetActive(true); },
        b => { b.ResetValues(); },
        b => { Destroy(b.gameObject, 0.5f); },
        false, 10, 20);

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (; ; )
        {
            var ball = _usePool ? _ballPool.Get() : Instantiate(_ball);
            ball.transform.parent = transform;
            ball.transform.position = transform.position;
            ball.Init(KillBall);
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void KillBall(Ball ball)
    {
        if (_usePool)
        {
            ball.ResetValues();
            _ballPool.Release(ball);
        }
        else
        {
            Destroy(ball.gameObject);
        }
    }
}
