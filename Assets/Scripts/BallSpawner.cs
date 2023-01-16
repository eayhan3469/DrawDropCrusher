using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private float _spawnRate;
    [SerializeField] private bool _usePool;

    private ObjectPool<Ball> _ballPool;
    private Sequence _spawnAnimationSequence;

    private void Start()
    {
        _ballPool = new ObjectPool<Ball>(() => { return Instantiate(_ball); },
        b => { b.gameObject.SetActive(true); },
        b => { b.ResetValues(); },
        b => { Destroy(b.gameObject, 0.5f); },
        false, 10, 20);
    }

    private void OnEnable()
    {
        GameManager.instance.OnGameStarted += StartSpawn;    
    }

    private void OnDisable()
    {
        GameManager.instance.OnGameStarted -= StartSpawn;
    }

    private void StartSpawn()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (GameManager.instance.CurrentGameState == GameManager.GameState.Started)
        {
            var ball = _usePool ? _ballPool.Get() : Instantiate(_ball);
            ball.transform.parent = transform;
            ball.transform.position = transform.position;
            ball.InitKillAction(KillBall);
            PlaySpawnAnimationEffect();
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private void PlaySpawnAnimationEffect()
    {
        _spawnAnimationSequence = DOTween.Sequence();
        _spawnAnimationSequence.Append(transform.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.2f));
        _spawnAnimationSequence.Append(transform.DOScale(new Vector3(0.8f, 1.2f, 1f), 0.2f));
        _spawnAnimationSequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
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
