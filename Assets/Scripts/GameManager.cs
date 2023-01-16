using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action OnGameStarted;
    public Action OnGameCompleted;

    public GameState CurrentGameState => _currentGameState;
    public int TotalCubeCount => _totalCubeCount;

    private GameState _currentGameState;
    private int _totalCubeCount;

    protected override void Awake()
    {
        _currentGameState = GameState.NotStarted;
        OnGameStarted += GameStarted;
        OnGameCompleted += GameCompleted;
    }

    private void OnDestroy()
    {
        OnGameStarted -= GameStarted;
        OnGameCompleted -= GameCompleted;
    }

    public void SetTotalCubeCount(int count)
    {
        _totalCubeCount = count;
    }

    private void GameCompleted()
    {
        _currentGameState = GameState.Completed;
    }

    private void GameStarted()
    {
        _currentGameState = GameState.Started;
    }

    public enum GameState
    {
        NotStarted,
        Started,
        Completed
    }
}
