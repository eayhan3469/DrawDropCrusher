using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _ballExplosionEffect;

    private void OnEnable()
    {
        Ball.OnBallExplode += BallExplosion;
    }

    private void OnDisable()
    {
        Ball.OnBallExplode -= BallExplosion;
    }

    private void BallExplosion(Ball ball)
    {
        Instantiate(_ballExplosionEffect, ball.transform.position, Quaternion.identity);

        Camera.main.DOShakePosition(0.1f, 0.4f, 10).OnComplete(() => DOTween.Rewind(Camera.main));
    }
}
